namespace StaticSiteGenerator.Test
{
    using Moq;
    using Shouldly;
    using StaticSiteGenerator.Builder;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Linq;
    using Xunit;

    public class SiteBuilderTest
    {
        private readonly IFileSystem fakeFileSystem;
        const string input = "./input";
        const string output = "./output";

        public SiteBuilderTest()
        {
            fakeFileSystem = new MockFileSystem();
        }

        [Fact]
        public void TestOutputFolderExist()
        {
            // Setup
            var fakeFilePath = fakeFileSystem.Path.Combine(output, "file.txt");
            fakeFileSystem.Directory.CreateDirectory(output);
            fakeFileSystem.File.WriteAllText(fakeFilePath, "Hola mundo");

            // Assert setup
            fakeFileSystem.File.Exists(fakeFilePath).ShouldBeTrue();

            var siteBuilder = new CLISiteBuilder(fakeFileSystem);


            // Act
            siteBuilder.CleanFolder(output);

            // Assert
            AssertDirectoryIsEmpty(output);
        }

        [Fact]
        public void TestOutputFolderDoesNotExist()
        {
            // Setup
            var siteBuilder = new CLISiteBuilder(fakeFileSystem);

            // Act
            siteBuilder.CleanFolder(output);

            // Assert
            AssertDirectoryIsEmpty(output);
        }

        private void AssertDirectoryIsEmpty(string output)
        {
            fakeFileSystem.Directory.Exists(output).ShouldBeTrue();
            fakeFileSystem.Directory.EnumerateFiles(output).Any().ShouldBeFalse();
            fakeFileSystem.Directory.EnumerateDirectories(output).Any().ShouldBeFalse();
        }

        [Fact]
        public void TestBuildCallsClean()
        {
            // Setup
            var mockSiteBuilder = new Mock<CLISiteBuilder>(MockBehavior.Strict, this.fakeFileSystem);
            mockSiteBuilder.Setup(x => x.CleanFolder(output));//Para que no se ejecute el contenido de la funcion, ya que no entra en el scope de la prueba
            var siteBuilder = mockSiteBuilder.Object;

            // Act
            siteBuilder.Build(input, output);

            // Assert
            mockSiteBuilder.Verify(x => x.CleanFolder(output));
        }
    }
}
