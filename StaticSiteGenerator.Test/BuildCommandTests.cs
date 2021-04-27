using StaticSiteGenerator.Commands;
using StaticSiteGenerator.Test.Utils;
using System;
using Xunit;
using Shouldly;

namespace StaticSiteGenerator.Test
{
    public class BuildCommandTests
    {
        [Fact]
        public void TestWriteToConsole()
        {
            // Setup
            var testConsole = new TestConsole();
            var buildCommand = new BuildCommand(testConsole);

            // Act
            buildCommand.OnExecute();

            // Assert
            string result = testConsole.GetWrittenContent();
            result.ShouldBe($"Se ejecuto el BuildCommand{Environment.NewLine}");
        }
    }
}
