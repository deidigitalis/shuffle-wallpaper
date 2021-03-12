namespace ShuffleWallpaper.Adapters
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IFileStreamAdapter
    {
        Task SaveAsync(Stream stream, string outputPath, CancellationToken cancellationToken);
    }
}
