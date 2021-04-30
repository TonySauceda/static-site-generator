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
            if (this.fileSystem.Directory.Exists(folder))
            {
                this.fileSystem.Directory.Delete(folder, true);
            }

            this.fileSystem.Directory.CreateDirectory(folder);
        }

        public void Build(string inputPath, string outputPath)
        {
            this.CleanFolder(outputPath);
        }
    }
}
