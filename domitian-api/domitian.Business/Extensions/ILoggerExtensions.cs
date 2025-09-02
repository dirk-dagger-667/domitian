using domitian.Business.Constants;
using domitian.Models.Results;
using Microsoft.Extensions.Logging;

namespace domitian.Business.Extensions
{
  public static class ILoggerExtensions
  {
    public static void LogResult<T>(this ILogger logger, Result result, string methodName, string serviceName, T input)
    {
      if (result.IsFailure)
      {
        logger.LogError(Messages.ExecStartTemplate, methodName, serviceName, input);
        logger.LogError(Messages.ExectFinishTemplate, methodName, serviceName, result);
      }
      else
      {
        logger.LogInformation(Messages.ExecStartTemplate, methodName, serviceName, input);
        logger.LogInformation(Messages.ExectFinishTemplate, methodName, serviceName, result);
      }
    }

    public static void LogResult<T1, T2>(this ILogger logger, T1 result, string methodName, string serviceName, T2 input)
    {
      if (result is null)
      {
        logger.LogError(Messages.ExecStartTemplate, methodName, serviceName, input);
        logger.LogError(Messages.ExectFinishTemplate, methodName, serviceName, result);
      }
      else
      {
        logger.LogInformation(Messages.ExecStartTemplate, methodName, serviceName, input);
        logger.LogInformation(Messages.ExectFinishTemplate, methodName, serviceName, result);
      }
    }
  }
}
