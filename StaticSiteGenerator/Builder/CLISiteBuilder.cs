namespace StaticSiteGenerator.Builder
{
    using System;
    using System.IO.Abstractions;

    public class CLISiteBuilder : ISiteBuilder
    {
        private readonly IFileSystem fileSystem;

        public CLISiteBuilder(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public virtual void CleanFolder(string folder)
        {
            if (this.fileSystem.Directory.Exists(folder))
            {
                this.fileSystem.Directory.Delete(folder, true);
            }

            this.fileSystem.Directory.CreateDirectory(folder);
        }

        public virtual void CopyFiles(string input,string output)
        {
            var source = this.fileSystem.DirectoryInfo.FromDirectoryName(input);
            var target = this.fileSystem.DirectoryInfo.FromDirectoryName(output);

            this.CopyFiles(source, target);
        }

        public void Build(string inputPath, string outputPath)
        {
            this.CleanFolder(outputPath);
            this.CopyFiles(inputPath, outputPath);
        }

        private void CopyFiles(IDirectoryInfo source, IDirectoryInfo target)
        {
            foreach (var file in source.GetFiles())
            {
                file.CopyTo(this.fileSystem.Path.Combine(target.FullName, file.Name));
            }

            foreach (var subDir in source.GetDirectories())
            {
                var nextTargetSubDir = target.CreateSubdirectory(subDir.Name);
                this.CopyFiles(subDir, nextTargetSubDir);
            }
        }
    }
}
