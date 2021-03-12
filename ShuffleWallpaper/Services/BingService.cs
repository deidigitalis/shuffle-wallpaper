namespace ShuffleWallpaper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Adapters;
    using DTOs;

    public class BingService : IBingService
    {
        private const string BingBaseUrl = "https://www.bing.com";
        private const string BingImageArchiveUrl = @"https://www.bing.com/HPImageArchive.aspx?format=js&idx={0}&n=10&mkt=en-US";
        private readonly IHttpAdapter _httpAdapter;
        private readonly IFileStreamAdapter _streamAdapter;

        public BingService(IHttpAdapter httpAdapter, IFileStreamAdapter streamAdapter)
        {
            _httpAdapter = httpAdapter;
            _streamAdapter = streamAdapter;
        }

        public async Task<BingArchiveDto> DownloadMetadataAsync(int idx)
        {
            string url = string.Format(CultureInfo.InvariantCulture, BingImageArchiveUrl, idx);
            return await _httpAdapter.GetFromJsonAsync<BingArchiveDto>(url);
        }

        public async Task<IEnumerable<string>> DownloadImagesAsync(BingArchiveDto metadata, string folderPath)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var images = GetDownloadInfo(metadata, folderPath).ToList();

            foreach ((Uri absoluteUri, string outputPath) in images)
            {
                try
                {
                    var imageStream = await _httpAdapter.GetStreamAsync(absoluteUri, cancellationTokenSource.Token);
                    await _streamAdapter.SaveAsync(imageStream, outputPath, cancellationTokenSource.Token);
                }
                catch (Exception)
                {
                    cancellationTokenSource.Cancel();
                    throw;
                }
            }

            return images.Select(x => x.OutputPath).ToList();
        }

        private static IEnumerable<(Uri AbsoluteUri, string OutputPath)> GetDownloadInfo(BingArchiveDto metadata, string folderPath)
        {
            const string FileNameKey = "id";

            var baseUri = new Uri(BingBaseUrl, UriKind.Absolute);

            return metadata.Images.Select(x => new
            {
                AbsoluteUri = new Uri(baseUri, x.Url),
                FileName = QueryStringParser.Parse(x.Url).TryGetValue(FileNameKey, out string id) ? id : null
            })
                .Where(x => !string.IsNullOrWhiteSpace(x.FileName))
                .Select(x => (x.AbsoluteUri, OutputPath: Path.Combine(folderPath, x.FileName)))
                .Where(x => !File.Exists(x.OutputPath));
        }
    }
}
