namespace domitian.Models.Results.Errors
{
  public record UsersErrors : Error
  {
    public UsersErrors(ErrorCodes Code,
      string? Message = null,
      string? DevErrorMessage = null,
      Exception? Ex = null)
      : base(Code, Message, DevErrorMessage, Ex) { }

    public static readonly Error UsersNotFound = new Error(ErrorCodes.UsersNotFound,
      DevOperationErrorMessages.UsersNotFound,
      DevOperationErrorMessages.UsersNotFound);
  }
}
