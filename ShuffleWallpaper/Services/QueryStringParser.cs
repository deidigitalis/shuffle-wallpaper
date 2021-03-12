namespace ShuffleWallpaper.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class QueryStringParser
    {
        public static IDictionary<string, string> Parse(string uriString)
        {
            int indexOfArguments = uriString.IndexOf('?');

            return uriString.Substring(indexOfArguments + 1)
                .Split('&', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split('=', StringSplitOptions.RemoveEmptyEntries))
                .Where(x => x.Length == 2)
                .ToDictionary(x => x[0], x => x[1]);
        }
    }
}
