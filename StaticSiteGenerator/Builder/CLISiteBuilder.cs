namespace StaticSiteGenerator.Builder
{
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Text;
    using DotLiquid;
    using Slugify;

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
            var slugHelper = new SlugHelper();

            foreach (var rawPost in rawPosts)
            {
                var post = this.SplitPost(rawPost);
                var metadata = this.ConvertMetadata(post.Item1);
                var content = post.Item2;

                var renderedPost = this.RenderContent(metadata, content, inputPath);
                var postSlug = slugHelper.GenerateSlug(metadata.Title);
                var outputPostPath = this.fileSystem.Path.Combine(outputPath, $"{postSlug}.html");

                this.fileSystem.File.WriteAllText(outputPostPath, renderedPost);
            }
        }

        public virtual IEnumerable<string> GetPosts(string inputPath)
        {
            var inputPostsPath = this.fileSystem.Path.Combine(inputPath, "posts");

            if (this.fileSystem.Directory.Exists(inputPostsPath))
            {
                foreach (var file in this.fileSystem.Directory.EnumerateFiles(inputPostsPath, "*.*", System.IO.SearchOption.AllDirectories))
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
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Split(':', 2))
                .Select(x => KeyValuePair.Create(x[0].Trim().ToLower(), x[1].Trim()));

            var dic = new Dictionary<string, string>(metadataEntries);

            return new RawPostMetadata()
            {
                Title = dic["title"],
                Date = DateTime.Parse(dic["date"]),
            };
        }

        public virtual string RenderContent(RawPostMetadata metadata, string content, string inputPath)
        {
            Template.FileSystem = new FileSystemHelper(this.fileSystem, inputPath);

            var contentWrapper = @"{% extends post %}
{% block post_content %}
" + content + @"
{% endblock %}";

            var template = Template.Parse(contentWrapper.ToString());

            var postVariables = new
            {
                title = metadata.Title,
                date = metadata.Date,
            };

            var rendetedContent = template.Render(Hash.FromAnonymousObject(postVariables));

            return rendetedContent;
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
