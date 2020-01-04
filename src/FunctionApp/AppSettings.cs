using System;
using Microsoft.Extensions.Configuration;

namespace FunctionApp
{
    public static class AppSettings
    {
        public static void Initialize(IConfiguration configuration)
        {
            FtpServer.UserName = configuration.GetValue<string>("FtpServer:UserName");
            FtpServer.Password = configuration.GetValue<string>("FtpServer:Password");
            FtpServer.FileUri = configuration.GetValue<string>("FtpServer:FileUri");

            StorageAccount.SharedAccessKey.Backups.Write = new Uri(configuration.GetValue<string>("StorageAccount:SharedAccessKey:Backups:Write"));
        }
        public static class FtpServer
        {
            public static string FileUri { get; internal set; }
            public static string UserName { get; internal set; }
            public static string Password { get; internal set; }
        }
        public static class StorageAccount
        {
            public static class SharedAccessKey
            {
                public static class Backups
                {
                    public static Uri Write { get; internal set; }
                }
            }
        }

    }
}
