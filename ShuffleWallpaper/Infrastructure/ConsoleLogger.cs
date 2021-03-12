namespace ShuffleWallpaper.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    public class ConsoleLogger : ILogger
    {
        private static readonly IDictionary<LogLevel, ConsoleColor> _customConsoleColors = new Dictionary<LogLevel, ConsoleColor>()
        {
            {LogLevel.Information, ConsoleColor.Yellow},
            {LogLevel.Debug, ConsoleColor.DarkGray},
            {LogLevel.None, ConsoleColor.DarkGray},
            {LogLevel.Trace, ConsoleColor.White},
            {LogLevel.Warning, ConsoleColor.DarkMagenta},
            {LogLevel.Error, ConsoleColor.Red},
            {LogLevel.Critical, ConsoleColor.DarkRed},
        };

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = _customConsoleColors[logLevel];
            Console.WriteLine($"{formatter.Invoke(state, exception)}");
            Console.ForegroundColor = previousColor;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
