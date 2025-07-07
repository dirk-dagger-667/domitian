namespace domitian.Models.Extensions
{
  public static class ExceptionExtesions
  {
    public static string GetInnermostExceptionMessage(this Exception ex)
        => ex.InnerException != null ? GetInnermostExceptionMessage(ex.InnerException) : ex.Message;
  }
}
