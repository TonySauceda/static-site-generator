using StaticSiteGenerator.Commands;
using StaticSiteGenerator.Test.Utils;
using System;
using Xunit;
using Shouldly;
using Moq;
using StaticSiteGenerator.Builder;

namespace StaticSiteGenerator.Test
{
    public class BuildCommandTests
    {
        [Fact]
        public void TestSiteBuilderBuildRuns()
        {
            // Setup
            var testConsole = new TestConsole();
            var mockSiteBuilder = new Mock<ISiteBuilder>();
            var buildCommand = new BuildCommand(testConsole, mockSiteBuilder.Object);

            // Act
            buildCommand.OnExecute();

            // Assert
            mockSiteBuilder.Verify(x => x.Build("./", "./_site"), Times.Once);
        }
    }
}
