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

        public int OnExecute()
        {
            this.console.WriteLine("Se ejecuto el BuildCommand");
            return 0;
        }
    }
}
