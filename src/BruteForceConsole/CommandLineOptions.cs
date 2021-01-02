using System;
using CommandLine;

namespace BruteForceConsole
{
    public class CommandLineOptions
    {
        [Option('u', "url", HelpText = "Simple url to brute force")]
        public string Url { get; set; }
        
        [Option("user-file", HelpText = "File that contains users")]
        public string UserFile { get; set; }

        [Option("password-file", HelpText = "File that contains passwords")]
        public string PasswordFile { get; set; }

        [Option("output-file", HelpText = "File that contains passwords")]
        public string OutputFile { get; set; }
    }
}