namespace ShuffleWallpaper.Adapters
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public class HttpAdapter : IHttpAdapter
    {
        public async Task<TDto> GetFromJsonAsync<TDto>(string requestUri)
        where TDto : class
        {
            using var httpClient = new HttpClient();

            if ((await httpClient.GetFromJsonAsync(requestUri, typeof(TDto))) is not TDto dto)
            {
                throw new InvalidCastException(string.Format(CultureInfo.CurrentUICulture, Strings.GetFromJsonFailure, typeof(TDto).Name));
            }

            return dto;
        }

        public async Task<Stream> GetStreamAsync(Uri absoluteUri, CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetStreamAsync(absoluteUri, cancellationToken);
        }
    }
}
