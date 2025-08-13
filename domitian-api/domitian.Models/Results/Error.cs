namespace domitian.Models.Results
{
  public record Error(ErrorCodes Code,
    string? Message = null,
    string? DevErrorMessage = null,
    Exception? Ex = null)
  {
    public static readonly Error None = new Error(ErrorCodes.NoError);

    public static readonly Error CreatedEntity = new Error(ErrorCodes.NoError);

    public static readonly Error Exception = new Error(ErrorCodes.Exception, ValidationErrorsMessages.CriticalError);

    //public static Error InvalidArgument(string argName) => new Error(ErrorCodes.GenericInvalidArgument, $"{DevOperationErrorMessages.InvalidArgument}: ${argName}");
  }

  public record RegisterErrors : Error
  {
    public RegisterErrors(ErrorCodes Code,
      string Message,
      string DevErrorMessage,
      Exception? Ex = null) : base(Code, Message, DevErrorMessage, Ex) { }

    public static readonly Error RegisterInvalidEmail = new Error(ErrorCodes.RegisterInvalidEmailError,
      DevOperationErrorMessages.InvalidEmail,
      DevOperationErrorMessages.InvalidEmail);

    //public static readonly Error RegisterUserExists = new Error(ErrorCodes.RegisterUserExistsError,
    //  PublicErrorMessages,
    //  DevOperationErrorMessages.RegisterUserExists);

    public static Error RegisterUserAddToRoleFails(string? email) => new Error(ErrorCodes.RegisterUserAddToRoleFails,
      string.Format(PublicErrorMessages.RegisterUserProblem, email),
      DevOperationErrorMessages.RegisterUserAddToRoleFails);

    public static Error RegisterUserExists(string? email) => new Error(ErrorCodes.RegisterInvalidEmailError,
        string.Format(PublicErrorMessages.RegisterUserProblem, email),
        DevOperationErrorMessages.RegisterUserExists);

    public static Error RegisterCreateAccount(string? email)
        => new Error(ErrorCodes.RegisterCreateError, $"{ValidationErrorsMessages.UnableToCreateAccountError} {email}.");
  }

  public record LoginErrors : Error
  {
    public LoginErrors(ErrorCodes Code,
      string Message,
      string DevErrorMessage,
      Exception? Ex = null) : base(Code, Message, DevErrorMessage, Ex) { }

    public static readonly Error LoginLockedOut = new Error(ErrorCodes.LoginLockedOutError,
      DevOperationErrorMessages.LoginFailed,
      DevOperationErrorMessages.LockedOut);

    public static readonly Error FailedAttempt = new Error(ErrorCodes.LoginFailedAttemptError,
      DevOperationErrorMessages.LoginFailed,
      DevOperationErrorMessages.LoginFailed);

    public static readonly Error WrongPassword = new Error(ErrorCodes.LoginWrongPasswordError,
      DevOperationErrorMessages.LoginFailed,
      DevOperationErrorMessages.LoginWrongPassword);

    public static Error LoginNotFound(string? email) => new Error(ErrorCodes.LoginNotFoundError,
      DevOperationErrorMessages.LoginFailed,
      $"{DevOperationErrorMessages.LoginNotFound} : {email}");
  }
}
