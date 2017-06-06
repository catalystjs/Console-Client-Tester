using System;
using System.Linq;
using System.Threading.Tasks;

namespace Console.Client.Core
{
    using CommandLineParser;
    using Handlers;
    using Models;
    using ActiveDirectory;
    class Program
    {
        private static readonly CommandLineParser CommandLineOptions = new CommandLineParser();
        static void Main(string[] args)
        {
            int exitCode = 0;

            try
            {
                // Check command line arguments
                if (!ValidateCommandLineParameters(args))
                {
                    if (args.Length != 1 || args[0] != "--help")
                    {
                        exitCode = -1;
                    }

                    return;
                }

                Task.Run(async () =>
                {
                    using (var client = Client.GetAuthenticatedClient(CommandLineOptions))
                    {
                        /* Send event as POST
                        var response = await client.PostAsync($"{local_apiURL}/event", stringContent);
                        */

                        // Send event as GET (Set URI for endpoint)
                        var response = await client.GetAsync($"{CommandLineOptions.URL}/event");

                        // Client response
                        var responseContent = await response.Content.ReadAsStringAsync();

                        System.Console.WriteLine("Send event response");
                        System.Console.WriteLine(responseContent);

                        if (response.IsSuccessStatusCode)
                        {
                            var apiResponse = JsonConvert.DeserializeObject<EventApiModel>(responseContent);

                            // Get event status
                            response = await client.GetAsync($"{CommandLineOptions.URL}/event/status/{apiResponse.CorrelationId}");
                            //response = await client.GetAsync($"{apiUrl_test}/event/status/{apiResponse.CorrelationId}");
                            //response = await client.GetAsync($"{apiUrl}/event/status/{apiResponse.CorrelationId}");
                            responseContent = await response.Content.ReadAsStringAsync();

                            System.Console.WriteLine("Event status response");
                            System.Console.WriteLine(responseContent);
                        }
                    }
                }).Wait();

            }
            catch (AggregateException ex)
            {
                System.Console.WriteLine(ex.InnerException.Message);
                exitCode = -1;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                exitCode = -1;
            }
            finally
            {
                Environment.Exit(exitCode);
            }
            //var local_apiURL = "https://localhost:44300/api/v1";
            //var staging_apiUrl = "https://emails-staging.azure.net/api/v1";
            //var apiUrl = "https://aeo-frontend-westus-dev-stable.azurewebsites.net/api/v1";
            //var apiUrl = "https://aeo-test-api-auth.azurewebsites.net/api/v1";
            //var apiUrl_test = "https://aeo-test-api-auth-integrated-aad.azurewebsites.net/api/v1";

            /* Example model for posting to API
            var model = new EventModel
            {
                Email = "v-jossum@microsoft.com",
                EventId = "test",
                Culture = "en-us",
                Values = new Dictionary<string, object>
                {
                    { "SubscriptionId", "e04f6872-d75b-447d-8549-23b40f6ab823" },
                    { "SubscriptionName", "Azure Text Subscription" }
                }
            };

            var jsonModel = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(jsonModel, Encoding.UTF8, "application/json");
            */
        }
        // Handle parsing of arguments
        private static bool ValidateCommandLineParameters(string[] args)
        {
            try
            {
                const string HelpAlias = "/?";
                args = args.Select(arg => arg.Equals(HelpAlias) ? "--help" : arg).ToArray();

                if (!CommandLine.Parser.Default.ParseArguments(args, CommandLineOptions))
                {
                    if (args.Length != 1 || args[0] != "--help")
                    {
                        System.Console.WriteLine("Command line parameters could not be parsed.");
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An exception occurred while parsing parameters. Details: {ex}");
                return false;
            }

            // CmdLineOpt.LogValues(logger.LogInfo);

            return true;
        }
    }
}
