namespace StaticSiteGenerator.Commands
{
    using System;

    public class BuildCommand
    {
        public int OnExecute()
        {
            Console.WriteLine("Se ejecuto el BuildCommand");
            return 0;
        }
    }
}
