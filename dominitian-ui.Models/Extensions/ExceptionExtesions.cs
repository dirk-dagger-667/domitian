namespace dominitian.Models.Extensions
{
    public static class ExceptionExtesions
    {
        public static string GetInnermostException(this Exception ex)
            => ex.InnerException != null ? GetInnermostException(ex.InnerException) : ex.Message;
    }
}
