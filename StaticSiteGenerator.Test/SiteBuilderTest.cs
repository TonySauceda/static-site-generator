namespace StaticSiteGenerator.Test
{
    using Moq;
    using Shouldly;
    using StaticSiteGenerator.Builder;
    using System;
    using System.Collections.Generic;
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

        [Fact]
        public void TestBuildCallsCleanAndCopy()
        {
            // Setup
            var mockSiteBuilder = new Mock<CLISiteBuilder>(MockBehavior.Strict, this.fakeFileSystem);
            mockSiteBuilder.Setup(x => x.CleanFolder(output));//Para que no se ejecute el contenido de la funcion, ya que no entra en el scope de la prueba
            mockSiteBuilder.Setup(x => x.CopyFiles(input, output));
            mockSiteBuilder.Setup(x => x.GetPosts(input)).Returns(new List<string>());
            mockSiteBuilder.Setup(x => x.SplitPost(It.IsAny<string>())).Returns(Tuple.Create("", ""));
            mockSiteBuilder.Setup(x => x.ConvertMetadata(It.IsAny<string>())).Returns(new RawPostMetadata());
            var siteBuilder = mockSiteBuilder.Object;

            // Act
            siteBuilder.Build(input, output);

            // Assert
            mockSiteBuilder.Verify(x => x.CleanFolder(output));
            mockSiteBuilder.Verify(x => x.CopyFiles(input, output));
        }

        [Fact]
        public void TestCopyFiles()
        {
            // Setup
            var stylesFile = fakeFileSystem.Path.Combine(input, "style.css");
            var someOtherFile = fakeFileSystem.Path.Combine(input, "subFolder", "sybStyle.css");

            var content = new Dictionary<string, MockFileData>
            {
                { stylesFile, new MockFileData("body { color: #fff; }") },
                { someOtherFile, new MockFileData("body { color: #aaa; }") }
            };

            var fakeFIleSystem = new MockFileSystem(content);
            fakeFIleSystem.Directory.CreateDirectory(output);
            var siteBuilder = new CLISiteBuilder(fakeFIleSystem);

            // Act
            siteBuilder.CopyFiles(input, output);

            // Assert
            fakeFIleSystem.Directory.EnumerateFiles(output, "*.*", System.IO.SearchOption.AllDirectories).Count().ShouldBe(2);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public void TestGetPostsWithSinglePost(int files)
        {
            var postsPath = this.fakeFileSystem.Path.Combine(input, "posts");
            this.fakeFileSystem.Directory.CreateDirectory(postsPath);
            var fileContets = new List<string>();
            for (int i = 0; i < files; i++)
            {
                var postPath = this.fakeFileSystem.Path.Combine(postsPath, $"file_{i}.txt");
                var content = $"# Hola mundo!\n\nPrueba: {i}";
                this.fakeFileSystem.File.WriteAllText(postPath, content);
                fileContets.Add(content);
            }

            var siteBuilder = new CLISiteBuilder(this.fakeFileSystem);

            var posts = siteBuilder.GetPosts(input);

            posts.ShouldBe(fileContets);
        }


        private void AssertDirectoryIsEmpty(string output)
        {
            fakeFileSystem.Directory.Exists(output).ShouldBeTrue();
            fakeFileSystem.Directory.EnumerateFiles(output).Any().ShouldBeFalse();
            fakeFileSystem.Directory.EnumerateDirectories(output).Any().ShouldBeFalse();
        }
    }
}
