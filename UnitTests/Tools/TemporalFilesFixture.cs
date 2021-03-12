namespace UnitTests.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class TemporalFilesFixture : IDisposable
    {
        private bool _disposedValue;
        private readonly ISet<string> _temporalFiles = new HashSet<string>();

        public TemporalFilesFixture()
        {
        }

        public string CreateTempFile()
        {
            string path = Path.GetTempFileName();
            _temporalFiles.Add(path);
            return path;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            foreach (string temporalFile in _temporalFiles)
            {
                File.Delete(temporalFile);
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
