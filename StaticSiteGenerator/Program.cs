namespace StaticSiteGenerator
{
    using System;
    using McMaster.Extensions.CommandLineUtils;

    [Command("Static Site Generator")]
    [VersionOptionFromMember("--version", MemberName = nameof(Version))]
    public class Program
    {
        public string Version { get; } = "0.0.0";

        public static void Main(string[] args)
        {
            CommandLineApplication.Execute<Program>(args);
        }

        public int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 0;
        }
    }
}
