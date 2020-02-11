using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using masked_emails.Commands;
using masked_emails.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Utils;

namespace masked_emails
{
    class Program
    {
        private static void Main(string[] args)
            => MainAsync(args)
            .GetAwaiter()
            .GetResult()
            ;

        private static async Task MainAsync(string[] args)
        {
            if (args.Length == 2 && args[0] == "--hash-password")
            {
                var plaintext = args[1];
                Console.WriteLine(PasswordHelper.HashPassword(plaintext));
                Environment.Exit(42);
            }

            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var environment = ConsoleHostEnvironment.Build();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appSettings.json")
                .AddJsonFile($"appSettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                .Build()
                ;

            var section = configuration.GetSection("AppSettings");
            var endpoints = new Endpoints();
            section.Bind(endpoints);

            Console.WriteLine($"masked-emails: Environment {environment.EnvironmentName}.");
            Console.WriteLine($"api: {endpoints.Api}."); 
            Console.WriteLine($"authority: {endpoints.Authority}."); 


            var cmdLine = CommandLine.Parse(args);

            var username = cmdLine.Username;
            var password = cmdLine.Password;

            var action = cmdLine.Action;
            var options = cmdLine.Args;

            if (String.IsNullOrEmpty(username))
            {
                Console.Out.WriteLine("Login:");
                username = Console.ReadLine();
            }
            if (String.IsNullOrEmpty(password))
            {
                Console.Out.WriteLine("Password:");
                password = ConsoleUtil.GetPassword();
                Console.Out.WriteLine();
            }

            {
                // add a --password-hash command line option
                // for create masked email command

                var passwordHash = PasswordHelper.HashPassword(password);
                options = options.Concat(new[] {"--password-hash", passwordHash,}).ToArray();
            }

            var credentials = new NetworkCredential(username, password);
            var client = new Client.MaskedEmailsClient(endpoints, credentials);

            try
            {
                await client.GetProfileAsync();
            }
            catch (HttpRequestException e)
            {
                Console.Error.WriteLine($"ERR: unable to connect to the masked-emails service.");
                Console.Error.WriteLine($"ERR: {e.Message}.");
                Environment.Exit(42);
            }

            if (action == Actions.Unrecognized)
            {
                new ReadEvalPrintLoop(client).Prompt(options);
            }
            else
            {
                await Command.ExecuteAsync(client, action, options);
            }
        }
    }

    public sealed class ConsoleHostEnvironment : IHostEnvironment
    {
        private ConsoleHostEnvironment()
        { }

        public static IHostEnvironment Build()
        {
            var environment = new ConsoleHostEnvironment();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Production";

            var assembly = Assembly.GetExecutingAssembly();

            environment.EnvironmentName = env;
            environment.ApplicationName = assembly.GetName().Name;
            environment.ContentRootPath = assembly.Location != null
                ? Path.GetDirectoryName(assembly.Location)
                : Environment.CurrentDirectory
                ;

            environment.ContentRootFileProvider =
                new PhysicalFileProvider(environment.ContentRootPath)
                ;

            return environment;
        }

        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}
