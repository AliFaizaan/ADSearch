using System;
using CommandLine;

namespace ADSearch {

    class Program {

        static void Main(string[] args) {
            var cmdOptions = Parser.Default.ParseArguments<Options>(args);
            cmdOptions.WithParsed(
                options => {
                    Entry(options);
                });
        }

        static void Entry(Options options) {
            ADWrapper AD;
            
            if (options.IP == null && options.Domain != null) {
                //No IP but domains set
                AD = new ADWrapper(options.Domain, options.Username, options.Password);
            } else if (options.IP != null && options.Domain != null) {
                //This requires the domain so it can be converted into a valid LDAP URI
                AD = new ADWrapper(options.Domain, options.IP, options.Port, options.Username, options.Password);
            } else {
                //When no domain is supplied it has to be done locally even if the ip is set otherwise the bind won't work
                OutputFormatting.PrintVerbose("No domain supplied. This PC's domain will be used instead");
                AD = new ADWrapper();
            }

            if (options.Groups) {
                OutputFormatting.PrintVerbose("ALL GROUPS: ");
                AD.ListAllGroups(options.Full);
            }
            
            if (options.Users) {
                OutputFormatting.PrintVerbose("ALL USERS: ");
                AD.ListAllUsers(options.Full);
            }

            if (options.Computers) {
                OutputFormatting.PrintVerbose("ALL COMPUTERS: ");
                AD.ListAllComputers(options.Full);
            }

            if (options.Search != null) {
                OutputFormatting.PrintVerbose("ALL SPNS: ");
                AD.ListCustomSearch(options.Search, options.Full);
            }

            if (options.Spns) {
                OutputFormatting.PrintVerbose("ALL SPNS: ");
                AD.ListAllSpns();
            }
        }

        private static void GetHelp() {
            Console.WriteLine("Please enter valid arguments");
        }
    }
}