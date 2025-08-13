namespace domitian.Models.Results
{
  public class DevOperationErrorMessages
  {
    #region Generic
    public const string NullUserId = "Unable to load user with ID.";
    public const string InvalidEmail = "Error confirming your email.";
    public const string OperationFailed = "Operation failed";
    public const string InvalidArgument = "Provide a valid argument";
    #endregion

    #region Register
    public const string RegisterUserExists = "User already exists.";
    public const string RegisterUserAddToRoleFails = "Can't assign role to user.";
    #endregion

    #region Login
    public const string LockedOut = "User account locked out.";
    public const string LoginFailed = "Something went wrong while trying to login.";
    public const string LoginNotFound = "User not found.";
    public const string LoginWrongPassword = "Wrong password.";
    public const string LoginUsernameNull = "Provide a valid username.";
    public const string LoginExpiredToken = "Access has expired. Please login again.";
    #endregion
  }

  public class PublicErrorMessages
  {
    public const string RegisterUserProblem = "Something went wrong while validating the username: {0}";
  }
}
