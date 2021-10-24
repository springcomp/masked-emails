using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class Events
    {
        private static readonly EventId backupSucceeded_ = new EventId(001, "backup-succeeded");
        private static readonly EventId backupFailed_ = new EventId(999, "backup-failed");

        public static EventId BackupFailed
            => backupFailed_;
        public static EventId BackupSucceeded
            => backupSucceeded_;
    }
}
