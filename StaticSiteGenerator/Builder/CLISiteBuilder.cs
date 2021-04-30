namespace StaticSiteGenerator.Builder
{
    using System.IO.Abstractions;

    public class CLISiteBuilder : ISiteBuilder
    {
        private readonly IFileSystem fileSystem;

        public CLISiteBuilder(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void CleanFolder(string folder)
        {

        }

        public void Build(string inputPath, string outputPath)
        {
            this.CleanFolder(outputPath);
        }
    }
}
