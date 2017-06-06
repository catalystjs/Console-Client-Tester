using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

namespace Console.Client.CommandLineParser
{
    public class CommandLineParser
    {
        [Option("URL", Required = true, HelpText = "FQDN of the site you want to connect to. This should include URL and URI")]
        public string URL { get; set; }
        [Option("client-id", Required = true, HelpText = "Client ID of the application you are going to identify as")]
        public string clientID { get; set; }
        [Option("client-secret", Required = true, HelpText = "Client secret of the application you are going to identify as")]
        public string clientSecret { get; set; }
        [Option("client-audience", Required = true, HelpText = "Client audience of the application you are going to identify as")]
        public string clientAudience { get; set; }
        [Option("client-directory", Required = true, HelpText = "Client directory of the application you are going to identify as")]
        public string clientDirectory { get; set; }

        // Help options
        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            help.AddOptions(this);
            return help;
        }
        public override string ToString()
        {
            foreach (var item in this.GetType().GetProperties())
            {
                System.Console.WriteLine(item);
                System.Console.WriteLine(item.GetValue(this));
            }
            return base.ToString();
        }

    }
}
