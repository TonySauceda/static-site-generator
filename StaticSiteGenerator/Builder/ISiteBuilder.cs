namespace StaticSiteGenerator.Builder
{
    public interface ISiteBuilder
    {
        void Build(string inputPath, string outputPath);
    }
}
