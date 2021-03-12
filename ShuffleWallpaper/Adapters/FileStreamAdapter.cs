namespace ShuffleWallpaper.Adapters
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class FileStreamAdapter : IFileStreamAdapter
    {
        public async Task SaveAsync(Stream stream, string outputPath, CancellationToken cancellationToken)
        {
            await using var outputFileStream = new FileStream(outputPath, FileMode.Create);
            await stream.CopyToAsync(outputFileStream, cancellationToken);
        }
    }
}
