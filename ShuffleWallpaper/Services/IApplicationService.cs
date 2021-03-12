namespace ShuffleWallpaper.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApplicationService
    {
        Task MainAsync(IList<string> args);
    }
}
