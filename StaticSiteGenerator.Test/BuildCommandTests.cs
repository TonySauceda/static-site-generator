using StaticSiteGenerator.Commands;
using StaticSiteGenerator.Test.Utils;
using System;
using Xunit;

namespace StaticSiteGenerator.Test
{
    public class BuildCommandTests
    {
        [Fact]
        public void TestWriteToConsole()
        {
            var testConsole = new TestConsole();
            var buildCommand = new BuildCommand(testConsole);

            buildCommand.OnExecute();

            string result = testConsole.GetWrittenContent();
            Assert.Equal($"Se ejecuto el BuildCommand{Environment.NewLine}", result);
        }
    }
}
