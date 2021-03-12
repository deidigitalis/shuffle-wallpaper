namespace ShuffleWallpaper.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DTOs;

    public interface IBingService
    {
        Task<BingArchiveDto> DownloadMetadataAsync(int idx);
        Task<IEnumerable<string>> DownloadImagesAsync(BingArchiveDto metadata, string folderPath);
    }
}
