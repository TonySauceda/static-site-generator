namespace StaticSiteGenerator.Test
{
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
    }
}
