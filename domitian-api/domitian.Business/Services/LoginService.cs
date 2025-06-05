using domitian.Business.Contracts;
using domitian.Models.Requests.Login;
using domitian.Models.Responses.Login;
using domitian.Models.Results;
using domitian_api.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace domitian.Business.Services
{
  public class LoginService(SignInManager<DomitianIDUser> _signInManager,
      ITokenService _tokenService,
      ILogger<LoginService> _logger) : ILoginService
  {
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
    {
      var user = await _signInManager.UserManager.FindByEmailAsync(loginRequest.Email);

      if (user == null)
        return Result<LoginResponse>.Failure(OperationErrorMessages.OperationFailed, ResultType.Unauthorized, LoginErrors.LoginNotFound(loginRequest.Email));

      var canSignIn = await _signInManager.UserManager.CheckPasswordAsync(user, loginRequest.Password);

      if (!canSignIn)
        return Result<LoginResponse>.Failure(OperationErrorMessages.OperationFailed, ResultType.Unauthorized, LoginErrors.WrongPassword);

      // This doesn't count login failures towards account lockout
      // To enable password failures to trigger account lockout, set lockoutOnFailure: true
      var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, loginRequest.RememberMe, lockoutOnFailure: false);

      if (result.Succeeded)
      {
        _logger.LogInformation("User logged in.");

        user.RefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiry = DateTime.UtcNow.AddHours(2);

        var loginResponse = new LoginResponse()
        {
          Email = user.Email,
          BearerToken = _tokenService.GenerateJwt(user),
          RefreshToken = user.RefreshToken
        };

        var updUserRes = await _signInManager.UserManager.UpdateAsync(user);

        return Result<LoginResponse>.Success(loginResponse);
      }

      if (result.IsLockedOut)
      {
        _logger.LogWarning(LoginErrors.LoginLockedOut.Message);

        return Result<LoginResponse>.Failure(OperationErrorMessages.OperationFailed, ResultType.Unauthorized, LoginErrors.LoginLockedOut);
      }

      return Result<LoginResponse>.Failure(OperationErrorMessages.OperationFailed, ResultType.BadRequest, LoginErrors.FailedAttempt);
    }

    public async Task<Result<LoginResponse>> RefreshAccessAsync(RefreshRequest refReq)
    {
      _logger.LogInformation("Refresh called");

      var principal = _tokenService.GetPrincipalFromExpiredToken(refReq.AccessToken);

      if (!(principal?.Identity?.Name is not null))
        return Result<LoginResponse>.Failure(OperationErrorMessages.OperationFailed, ResultType.Unauthorized);

      var user = await _signInManager.UserManager.FindByNameAsync(principal.Identity.Name);

      if (user is null
          || user.RefreshToken != refReq.RefreshToken
          || user.RefreshTokenExpiry < DateTime.UtcNow)
        return Result<LoginResponse>.Failure(OperationErrorMessages.OperationFailed, ResultType.Unauthorized);

      var token = _tokenService.GenerateJwt(user);

      _logger.LogInformation("Refresh succeeded");

      return Result<LoginResponse>.Success(new LoginResponse()
      {
        BearerToken = token,
        Email = user.Email,
        RefreshToken = refReq.RefreshToken
      });
    }

    public async Task<Result> RevokeAccessAsync(string? username)
    {
      _logger.LogInformation("Revoke called");

      if (username is null)
        return Result.Failure(OperationErrorMessages.OperationFailed, ResultType.Unauthorized);

      var user = await _signInManager.UserManager.FindByNameAsync(username);

      if (user is null)
        return Result.Failure(OperationErrorMessages.OperationFailed, ResultType.Unauthorized);

      user.RefreshToken = null;
      user.RefreshTokenExpiry = DateTime.MinValue;

      var idResult = await _signInManager.UserManager.UpdateAsync(user);

      if (idResult is null || !idResult.Succeeded)
        return Result.Failure(OperationErrorMessages.OperationFailed, ResultType.Unauthorized);

      _logger.LogInformation("Revoke succeeded");

      return Result.Success();
    }
  }
}
