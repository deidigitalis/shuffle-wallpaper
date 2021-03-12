namespace ShuffleWallpaper.Adapters
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IHttpAdapter
    {
        Task<TDto> GetFromJsonAsync<TDto>(string requestUri)
            where TDto : class;

        Task<Stream> GetStreamAsync(Uri absoluteUri, CancellationToken cancellationToken);
    }
}
