namespace StaticSiteGenerator
{
    using System.IO.Abstractions;
    using McMaster.Extensions.CommandLineUtils;
    using Microsoft.Extensions.DependencyInjection;
    using StaticSiteGenerator.Builder;
    using StaticSiteGenerator.Commands;

    [Command("Static Site Generator")]
    [VersionOptionFromMember("--version", MemberName = nameof(Version))]
    [Subcommand(typeof(BuildCommand))]
    public class Program
    {
        public string Version { get; } = "0.0.5";

        public static int Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<ISiteBuilder, CLISiteBuilder>()
                .AddSingleton<IConsole>(PhysicalConsole.Singleton)
                .BuildServiceProvider();

            var app = new CommandLineApplication<Program>();
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services);

            return app.Execute(args);
        }

        public int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 0;
        }
    }
}
