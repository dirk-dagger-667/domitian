namespace domitian.Models.Results
{
    public record Error(ErrorCodes Code, string? Message = null, Exception? Ex = null)
    {
        public static readonly Error None = new Error(ErrorCodes.NoError);

        public static readonly Error CreatedEntity = new Error(ErrorCodes.NoError);

        public static readonly Error Exception = new Error(ErrorCodes.Exception, ValidationErrorsMessages.CriticalError);

        public static Error InvalidArgument(string argName) => new Error(ErrorCodes.GenericInvalidArgument, $"{OperationErrorMessages.InvalidArgument}: ${argName}");
    }

    public record RegisterErrors: Error
    {
        public RegisterErrors(ErrorCodes Code, string Message, Exception? Ex = null)
            : base(Code, Message, Ex) { }

        public static readonly Error RegisterUserNull = new Error(ErrorCodes.RegisterUserNullError, OperationErrorMessages.NullUserId);

        public static readonly Error RegisterInvalidEmail = new Error(ErrorCodes.RegisterInvalidEmailError, OperationErrorMessages.InvalidEmail);

        public static readonly Error RegisterUserExists = new Error(ErrorCodes.RegisterUserExistsError, OperationErrorMessages.RegisterUserExists);

        public static readonly Error RegisterUserAddToRoleFails = new Error(ErrorCodes.RegisterUserAddToRoleFails, OperationErrorMessages.RegisterUserAddToRoleFails);

        public static Error RegisterCreateAccount(string? email)
            => new Error(ErrorCodes.RegisterCreateError, $"{ValidationErrorsMessages.UnableToCreateAccountError} {email}.");
    }

    public record LoginErrors: Error
    {
        public LoginErrors(ErrorCodes Code, string Message, Exception? Ex = null)
            : base(Code, Message, Ex) { }

        public static readonly Error LoginLockedOut = new Error(ErrorCodes.LoginLockedOutError, OperationErrorMessages.LockedOut);

        public static readonly Error FailedAttempt = new Error(ErrorCodes.LoginFailedAttemptError, OperationErrorMessages.LoginFailed);

        public static readonly Error WrongPassword = new Error(ErrorCodes.LoginWrongPasswordError, OperationErrorMessages.LoginWrongPassword);

        public static Error LoginNotFound(string? email) => new Error(ErrorCodes.LoginNotFoundError, $"{OperationErrorMessages.LoginNotFound} : {email}");
    }
}
