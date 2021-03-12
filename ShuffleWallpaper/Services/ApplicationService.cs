namespace ShuffleWallpaper.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class ApplicationService : IApplicationService
    {
        private readonly ILogger _logger;
        private readonly IBingService _bingService;
        private const int FirstIdx = 0;
        private const int NoIdxs = 8;

        public ApplicationService(ILogger logger, IBingService bingService)
        {
            _logger = logger;
            _bingService = bingService;
        }

        public async Task MainAsync(IList<string> args)
        {
            string folderPath = args[0];

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            int counter = 0;
            foreach (int idx in Enumerable.Range(FirstIdx, NoIdxs))
            {
                var metadata = await _bingService.DownloadMetadataAsync(idx);
                var outputPaths = await _bingService.DownloadImagesAsync(metadata, folderPath);

                foreach (string outputPath in outputPaths)
                {
                    _logger.LogInformation(string.Format(CultureInfo.CurrentUICulture, Strings.DownloadedNotification, outputPath));
                    counter++;
                }
            }

            if (counter == 0)
            {
                _logger.LogWarning(Strings.NoImageDownloaded);
            }
        }
    }
}
