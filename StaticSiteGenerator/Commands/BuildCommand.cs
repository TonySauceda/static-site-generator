namespace StaticSiteGenerator.Commands
{
    using McMaster.Extensions.CommandLineUtils;
    using StaticSiteGenerator.Builder;

    public class BuildCommand
    {
        private readonly IConsole console;
        private readonly ISiteBuilder siteBuilder;

        public BuildCommand(IConsole console, ISiteBuilder siteBuilder)
        {
            this.console = console;
            this.siteBuilder = siteBuilder;
        }

        [Option("-r|--root")]
        public string InputPath { get; set; } = "./";

        [Option("-o|--output")]
        public string OutputPath { get; set; } = "./_site";

        public int OnExecute()
        {
            this.siteBuilder.Build(this.InputPath, this.OutputPath);

            this.console.WriteLine("Se ejecuto el BuildCommand");
            return 0;
        }
    }
}
