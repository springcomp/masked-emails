using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Client
{
    // https://identityserver4.readthedocs.io/en/latest/quickstarts/2_resource_owner_passwords.html 

    class Program
    {
        static void Main(string[] args)
            => MainAsync(args)
                .GetAwaiter()
                .GetResult();

        static async Task MainAsync(string[] args)
        {
            var server = "http://localhost:5000";
            var username = "alice";
            var password = "password";
            var requestUri = "http://localhost:5001/api/claims";

            if (args.Length == 2)
            {
                username = args[0];
                password = args[1];
            }
            if (args.Length >= 3)
            {
                server = args[0];
                username = args[1];
                password = args[2];

                if (args.Length == 4)
                    requestUri = args[3];
            }
            var tokenResponse = await RequestPasswordAccessToken(server, username, password);
            if (tokenResponse == null)
            {
                Console.Error.WriteLine("An error occurred while requesting a token.");
                return;
            }

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                Console.Error.WriteLine(response.StatusCode);
            }

            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
        }

        private static async Task<TokenResponse> RequestPasswordAccessToken(string server, string username, string password)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(server);
            if (disco.IsError)
            {
                Console.Error.WriteLine(disco.Error);
                return null;
            }

            var tokenResponse = await client.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "client.ro",
                    ClientSecret = "please-type-the-secret-here",

                    UserName = username,
                    Password = password,

                    Scope = "api",
                });

            if (tokenResponse.IsError)
            {
                Console.Error.WriteLine(tokenResponse.Error);
                return null;
            }

            Console.WriteLine(tokenResponse.Json);

            return tokenResponse;
        }
    }
}
