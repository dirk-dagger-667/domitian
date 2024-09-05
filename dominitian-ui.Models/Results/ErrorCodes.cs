namespace dominitian_ui.Models.Results
{
    public enum ErrorCodes
    {
        NoError,
        Exception,
        GenericInvalidArgument,

        #region Register service

        RegisterCreateError,
        RegisterUserNullError,
        RegisterInvalidEmailError,
        RegisterUserExistsError,
        RegisterUserAddToRoleFails,

        #endregion

        #region Login service

        LoginLockedOutError,
        LoginFailedAttemptError,
        LoginNotFoundError,
        LoginWrongPasswordError,
        #endregion
    }
}
