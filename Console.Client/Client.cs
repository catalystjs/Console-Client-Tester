using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Console.Client.ActiveDirectory
{
    using CommandLineParser;
    public class Client
    {
        public static HttpClient GetAuthenticatedClient(CommandLineParser CommandLineOptions)
        {
            var httpClientHandler = new HttpClientHandler { AllowAutoRedirect = false };
            var client = new HttpClient(httpClientHandler);

            var token = GetAdToken(CommandLineOptions.clientID, CommandLineOptions.clientSecret, CommandLineOptions.clientAudience, CommandLineOptions.clientDirectory);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // We assume this is an API call
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private static string GetAdToken(string clientId, string clientSecretKey, string resource, string directory)
        {
            var authenticationContext = new AuthenticationContext(string.Format("https://login.microsoftonline.com/{0}", directory));

            var credential = new ClientCredential(clientId, clientSecretKey);
            var result = authenticationContext.AcquireTokenAsync(resource, credential).Result;

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the AD token. Something went wrong");
            }
            System.Console.WriteLine("This is the token received");
            System.Console.WriteLine(result.AccessToken);

            return result.AccessToken;
        }
    }
}
