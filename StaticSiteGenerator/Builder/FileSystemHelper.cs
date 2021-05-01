namespace StaticSiteGenerator.Builder
{
    public class FileSystemHelper : DotLiquid.FileSystems.IFileSystem
    {
        private readonly System.IO.Abstractions.IFileSystem fileSystem;
        private readonly string rootPath;

        public FileSystemHelper(System.IO.Abstractions.IFileSystem fileSystem, string rootPath)
        {
            this.fileSystem = fileSystem;
            this.rootPath = rootPath;
        }

        public string ReadTemplateFile(DotLiquid.Context context, string templateName)
        {
            var templatePath = this.fileSystem.Path.Combine(this.rootPath, "templates", $"_{templateName}.liquid");
            return this.fileSystem.File.ReadAllText(templatePath);
        }
    }
}
