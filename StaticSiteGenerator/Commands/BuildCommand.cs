namespace StaticSiteGenerator.Commands
{
    using McMaster.Extensions.CommandLineUtils;

    public class BuildCommand
    {
        private readonly IConsole console;

        public BuildCommand(IConsole console)
        {
            this.console = console;
        }

        [Option("-r|--root")]
        public string InputPath { get; set; } = "./";

        [Option("-o|--output")]
        public string OutputPath { get; set; } = "./_site";

        public int OnExecute()
        {
            this.console.WriteLine("Se ejecuto el BuildCommand");
            return 0;
        }
    }
}
