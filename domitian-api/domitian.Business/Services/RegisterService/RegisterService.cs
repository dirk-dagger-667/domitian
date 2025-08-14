using domitian.Business.Contracts;
using domitian.Models.Requests.Registration;
using domitian.Models.Results;
using domitian_api.Data.Identity;
using DomitianDataConst = domitian.Data.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Encodings.Web;
using domitian.Infrastructure.Configuration.Authentication;

namespace domitian.Business.Services.RegisterService
{
  public class RegisterService(UserManager<DomitianIDUser> _userManager,
          IUserStore<DomitianIDUser> _userStore,
          SignInManager<DomitianIDUser> _signInManager,
          IEmailSender _emailSender,
          ILogger<RegisterService> _logger,
          IOptionsMonitor<ApiUrlOptions> _urlOptions) : IRegisterService
  {
    public async Task<Result<string>> RegisterAsync(RegisterRequest request)
    {
      var existingUser = await _userManager.FindByEmailAsync(request.Email);

      if (existingUser != null)
        return Result<string>.Failure(DevOperationErrorMessages.OperationFailed, ResultType.Conflict, RegisterErrors.RegisterUserExists);

      var user = CreateUser();

      await _userStore.SetUserNameAsync(user, request.Email, CancellationToken.None);

      if (_userManager.SupportsUserEmail)
      {
        IUserEmailStore<DomitianIDUser> _emailStore = GetEmailStore();
        await _emailStore.SetEmailAsync(user, request.Email, CancellationToken.None);
      }

      var createResult = await _userManager.CreateAsync(user, request.Password);

      if (createResult.Succeeded)
      {
        _logger.LogInformation("User created a new account with password.");

        var addedToRole = await _userManager.AddToRoleAsync(user, DomitianDataConst.UserRoles.FreeUser);

        if (!addedToRole.Succeeded)
          return Result<string>.Failure(DevOperationErrorMessages.OperationFailed, ResultType.BadRequest, RegisterErrors.RegisterUserAddToRoleFails);

        var callbackUrl = await BuildCallbackUrlAsync(user);

        if (!string.IsNullOrWhiteSpace(callbackUrl))
          await _emailSender.SendEmailAsync(request.Email, "Confirm your email",
          $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        if (_userManager.Options.SignIn.RequireConfirmedAccount)
          return Result<string>.Created(callbackUrl);
        else
        {
          await _signInManager.SignInAsync(user, isPersistent: false);

          return Result<string>.Created(string.Empty);
        }
      }

      return Result<string>.Failure(DevOperationErrorMessages.OperationFailed, ResultType.BadRequest, RegisterErrors.RegisterCreateAccount);
    }

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
      var user = await _userManager.FindByIdAsync(request.UserId);

      if (user == null)
        return Result.Failure(DevOperationErrorMessages.OperationFailed, ResultType.NotFound, LoginErrors.LoginNotFound);

      var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
      var emailValidResult = await _userManager.ConfirmEmailAsync(user, code);

      if (!emailValidResult.Succeeded)
        return Result.Failure(DevOperationErrorMessages.OperationFailed, ResultType.BadRequest, RegisterErrors.RegisterInvalidEmail);

      return Result.Success();
    }

    public async Task<Result<string>> ConfirmRegistrationAsync(string email)
    {
      var user = await _userManager.FindByEmailAsync(email);

      if (user == null)
        return Result<string>.Failure(DevOperationErrorMessages.OperationFailed, ResultType.Unauthorized, LoginErrors.LoginNotFound);

      var callbackUrl = await BuildCallbackUrlAsync(user);

      return Result<string>.Success(callbackUrl);
    }

    private DomitianIDUser CreateUser()
    {
      try
      {
        return Activator.CreateInstance<DomitianIDUser>();
      }
      catch
      {
        throw new InvalidOperationException($"Can't create an instance of '{nameof(DomitianIDUser)}'. " +
            $"Ensure that '{nameof(DomitianIDUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
            $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
      }
    }

    private IUserEmailStore<DomitianIDUser> GetEmailStore()
    {
      if (!_userManager.SupportsUserEmail)
        throw new NotSupportedException("The default UI requires a user store with email support.");

      return (IUserEmailStore<DomitianIDUser>)_userStore;
    }

    private async Task<string> BuildCallbackUrlAsync(DomitianIDUser user)
    {
      var userId = await _userManager.GetUserIdAsync(user);
      var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

      var callbackUrl = _urlOptions.CurrentValue.RegisterConfirmEmail(userId, code);

      return callbackUrl;
    }
  }
}
