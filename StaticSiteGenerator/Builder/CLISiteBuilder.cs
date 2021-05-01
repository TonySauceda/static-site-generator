﻿namespace StaticSiteGenerator.Builder
{
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Text;

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

        public virtual void CopyFiles(string input, string output)
        {
            var source = this.fileSystem.DirectoryInfo.FromDirectoryName(input);
            var target = this.fileSystem.DirectoryInfo.FromDirectoryName(output);

            this.CopyFiles(source, target);
        }

        public void Build(string inputPath, string outputPath)
        {
            this.CleanFolder(outputPath);
            this.CopyFiles(inputPath, outputPath);

            var rawPosts = this.GetPosts(inputPath);

            foreach (var rawPost in rawPosts)
            {
                var post = this.SplitPost(rawPost);
                var metadata = this.ConvertMetadata(post.Item1);
                var content = post.Item2;
            }
        }

        public virtual IEnumerable<string> GetPosts(string inputPath)
        {
            var inputPostsPath = this.fileSystem.Path.Combine(inputPath, "posts");

            if (this.fileSystem.Directory.Exists(inputPostsPath))
            {
                foreach (var file in this.fileSystem.Directory.EnumerateFiles(inputPath, "*.*", System.IO.SearchOption.AllDirectories))
                {
                    yield return this.fileSystem.File.ReadAllText(file);
                }
            }
        }

        public virtual Tuple<string, string> SplitPost(string post)
        {
            var metadata = new StringBuilder();
            var content = new StringBuilder();
            int count = 0;
            const string separator = "---";

            foreach (var line in post.Split(Environment.NewLine))
            {
                if (count == 2)
                {
                    content.Append(line).AppendLine();
                }
                else if (line == separator)
                {
                    count++;
                }
                else
                {
                    metadata.Append(line).AppendLine();
                }
            }

            return Tuple.Create(metadata.ToString(), content.ToString());
        }

        public virtual RawPostMetadata ConvertMetadata(string metadata)
        {
            var metadataEntries = metadata
                .Split(Environment.NewLine)
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Split(':', 2))
                .Select(x => KeyValuePair.Create(x[0].Trim().ToLower(), x[1].Trim()));

            var dic = new Dictionary<string, string>(metadataEntries);

            return new RawPostMetadata()
            {
                Title = dic["title"],
                Date = DateTime.Parse(dic["date"]),
            };
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
