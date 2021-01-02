using System;
using System.Threading.Tasks;
using CommandLine;
using Serilog;
using Serilog.Events;

namespace BruteForceConsole
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var arguments = Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsedAsync<CommandLineOptions>(async o =>
                {
                    await Run(o);
                });
        }

        private static async Task Run(CommandLineOptions commandLineOptions)
        {
            ConfigureSerilog(commandLineOptions);
            
            //ok create everything you need to test stuff
            //TODO: needs to finish console.
        }

        private static void ConfigureSerilog(CommandLineOptions commandLineOptions)
        {
            var config = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(("Log.txt"), LogEventLevel.Error)
                .CreateLogger();
        }
    }
}