using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSiteGenerator.Test.Utils
{
    public class TestConsole : IConsole
    {
        private readonly MemoryStream outStream;

        public TestConsole()
        {
            this.outStream = new MemoryStream();
            this.Out = new StreamWriter(this.outStream);
        }

        public string GetWrittenContent()
        {
            this.Out.Flush();
            this.outStream.Flush();
            this.outStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(this.outStream);
            return reader.ReadToEnd();
        }

        public TextWriter Out { get; private set; }

        public TextWriter Error => throw new NotImplementedException();

        public TextReader In => throw new NotImplementedException();

        public bool IsInputRedirected => throw new NotImplementedException();

        public bool IsOutputRedirected => throw new NotImplementedException();

        public bool IsErrorRedirected => throw new NotImplementedException();

        public ConsoleColor ForegroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ConsoleColor BackgroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event ConsoleCancelEventHandler CancelKeyPress;

        public void ResetColor()
        {
            throw new NotImplementedException();
        }
    }
}
