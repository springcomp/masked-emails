using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
namespace FunctionApp
{
    public class Functions
	{
		public Functions(IConfiguration configuration)
		{
			AppSettings.Initialize(configuration);
		}

		[FunctionName("backup-masked-emails-on-timer")]
		public async Task Run(
            [TimerTrigger("0 0 0 * * *")] TimerInfo myTimer,
            ILogger log
        )
		{
			var sasWrite = AppSettings.StorageAccount.SharedAccessKey.Backups.Write;

			try
			{
				await BackupFileToBlobAsync(sasWrite);
			}
			catch (RequestFailedException e)
			{
				if (e.Status == (int)HttpStatusCode.NotFound)
				{
					// create initial backup blob

                    var accessCondition = new BlockBlobOpenWriteOptions {
                        OpenConditions = new BlobRequestConditions{
                            IfNoneMatch = ETag.All,
                        },
                    };
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
                    log.LogBackupFailed(e.Message);
					throw;
				}
			}

            log.LogBackupSucceeded("Backing up profile database succeeded.");
		}

		private async Task BackupFileToBlobAsync(Uri sasWrite)
		{
			// create non existing blob

            var accessCondition = new BlockBlobOpenWriteOptions{
                OpenConditions = new BlobRequestConditions {
                    IfNoneMatch = ETag.All,
                }
            };

			await WriteBlobAndSnapshotAsync(sasWrite, accessCondition);
		}

		private async Task WriteBlobAndSnapshotAsync(Uri sasWrite, BlockBlobOpenWriteOptions blobRequestOptions)
		{
			var user = AppSettings.FtpServer.UserName;
			var password = AppSettings.FtpServer.Password;
			var filePath = AppSettings.FtpServer.FileUri;

			var blobName = Path.GetFileName(filePath);
            blobName = $"{DateTime.UtcNow:yyyy-MM-ddThh-mm-ss}_{blobName}";

			var container = new BlobContainerClient(sasWrite);
			var blob = container.GetBlockBlobClient(blobName);

			using (var source = await DownloadFtpFileAsync(filePath, user, password))
			using (var target = await blob.OpenWriteAsync(true, blobRequestOptions))
				source.CopyTo(target);
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
