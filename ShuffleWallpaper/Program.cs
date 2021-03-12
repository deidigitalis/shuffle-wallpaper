namespace ShuffleWallpaper
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Adapters;
    using Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Services;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddSingleton<IHttpAdapter, HttpAdapter>()
                .AddSingleton<IFileStreamAdapter, FileStreamAdapter>()
                .AddSingleton<IBingService, BingService>()
                .AddSingleton<IApplicationService, ApplicationService>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger>();

            AssemblyName name = Assembly.GetExecutingAssembly().GetName();

            logger.LogTrace($"{name.Name} {name.Version} - {Strings.Copyright}");

            if (args.Length != 1)
            {
                string message = string.Format(CultureInfo.CurrentUICulture, Strings.ArgumentHelp, name.Name);
                logger.LogInformation(message);
            }

            try
            {
                serviceProvider.GetRequiredService<IApplicationService>()
                    .MainAsync(args)
                    .Wait(TimeSpan.FromMinutes(1));
            }
            catch (AggregateException agEx)
            {
                foreach (Exception innerException in agEx.InnerExceptions)
                {
                    logger.LogError(innerException.Message);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
