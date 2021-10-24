using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public static class FunctionLoggingExtensions
    {
        public static void LogBackupSucceeded(this ILogger logger, string message)
        {
            logger.LogInformation(
                Events.BackupSucceeded,
                GlobalConstants.LogMessageTemplate,
                new[] {
                    message,
                }
            );
        }
        public static void LogBackupFailed(this ILogger logger, string message)
        {
            logger.LogError(
                Events.BackupFailed,
                GlobalConstants.LogMessageTemplate,
                new[] {
                    message,
                }
            );
        }
    }
}
