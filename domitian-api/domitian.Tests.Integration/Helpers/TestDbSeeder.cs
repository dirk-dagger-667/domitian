using domitian.Data.Constants;
using domitian.Models.Requests.Registration;
using domitian_api.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace domitian.Tests.Integration.Helpers
{
  public class TestDbSeeder
  {
    private readonly SignInManager<DomitianIDUser> _signInManager;
    private readonly IUserStore<DomitianIDUser> _userStore;
    private readonly IUserEmailStore<DomitianIDUser>? _emailStore;

    public TestDbSeeder(
      SignInManager<DomitianIDUser> signInManager,
      IUserStore<DomitianIDUser> userStore)
    {
      _signInManager = signInManager;
      _userStore = userStore;
      _emailStore = (IUserEmailStore<DomitianIDUser>)userStore;
    }

    public async Task BuildUserAsync(RegisterRequest registerRequest,
      bool confirmedEmail = false,
      DateTime? refreshTokenExpiry = null)
    {
      var user = Activator.CreateInstance<DomitianIDUser>();
      await _userStore.SetUserNameAsync(user, registerRequest.Email, CancellationToken.None);

      if (_signInManager.UserManager.SupportsUserEmail)
        await _emailStore?.SetEmailAsync(user, registerRequest.Email, CancellationToken.None);

      await _signInManager.UserManager.CreateAsync(user, registerRequest.Password);
      await _signInManager.UserManager.AddToRoleAsync(user, UserRoles.FreeUser);

      if (confirmedEmail)
      {
        var userId = await _signInManager.UserManager.GetUserIdAsync(user);
        var code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        await _signInManager.UserManager.ConfirmEmailAsync(user, code);
      }
    }
  }
}
