using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class Functions
    {
        public Functions(IConfiguration configuration)
        {
            AppSettings.Initialize(configuration);
        }

        [FunctionName("backup-masked-emails-on-timer")]
        public async Task Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            var sasWrite = AppSettings.StorageAccount.SharedAccessKey.Backups.Write;

            try
            {
                await BackupFileToBlobAsync(sasWrite);
            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusCode == (int)HttpStatusCode.NotFound)
                {
                    // create initial backup blob

                    var accessCondition = AccessCondition.GenerateIfNotExistsCondition();
                    await WriteBlobAndSnapshotAsync(sasWrite, accessCondition);

                    // there could be a race condition
                    // with the file being created between the time
                    // the exception has been raised and the call to WriteBlobAndSnapshotAsync.

                    // we do not deal with this condition here
                    // because in our case, it will never happen
                    // due to the file having been uploaded initially
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task BackupFileToBlobAsync(Uri sasWrite)
        {
            // replace previous existing blob

            var accessCondition = AccessCondition.GenerateIfMatchCondition("*");

            await WriteBlobAndSnapshotAsync(sasWrite, accessCondition);
        }

        private async Task WriteBlobAndSnapshotAsync(Uri sasWrite, AccessCondition accessCondition, BlobRequestOptions blobRequestOptions = null, OperationContext operationContext = null)
        {
            blobRequestOptions = blobRequestOptions ?? new BlobRequestOptions();
            operationContext = operationContext ?? new OperationContext();

            var user = AppSettings.FtpServer.UserName;
            var password = AppSettings.FtpServer.Password;
            var filePath = AppSettings.FtpServer.FileUri;

            var blobName = Path.GetFileName(filePath);
            var container = new CloudBlobContainer(sasWrite);
            var blob = container.GetBlockBlobReference(blobName);

            using (var source = await DownloadFtpFileAsync(filePath, user, password))
            using (var target = await blob.OpenWriteAsync(accessCondition, blobRequestOptions, operationContext))
                source.CopyTo(target);

            // make snapshot of current blob

            await blob.CreateSnapshotAsync();
        }

        private async Task<Stream> DownloadFtpFileAsync(string path, string user, string password)
        {
            var request = (FtpWebRequest)WebRequest.Create(path);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(user, password);

            var response = await request.GetResponseAsync();
            return response.GetResponseStream();
        }
    }
}
