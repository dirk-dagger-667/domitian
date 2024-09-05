namespace dominitian_ui.Models.Results
{
    public class OperationErrorMessages
    {
        #region Generic
        public const string NullUserId = "Unable to load user with ID.";
        public const string InvalidEmail = "Error confirming your email.";
        public const string InvalidArgument = "Provide a valid argument";
        #endregion

        #region Register
        public const string RegisterUserExists = "User already exists.";
        public const string RegisterUserAddToRoleFails = "Can't assign role to user.";
        #endregion

        #region Login
        public const string LockedOut = "User account locked out.";
        public const string LoginFailed = "Invalid login attempt.";
        public const string LoginNotFound = "User not found.";
        public const string LoginWrongPassword = "Wrong password.";
        public const string LoginUsernameNull = "Provide a valid username.";
        public const string LoginExpiredToken = "Access has expired. Please login again.";
        #endregion
    }
}
