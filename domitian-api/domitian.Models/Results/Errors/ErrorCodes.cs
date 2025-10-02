namespace domitian.Models.Results.Errors
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

    #region MyRegion
    UsersNotFound
    #endregion
  }
}
