namespace UnitTests.Tools
{
    using System;
    using Moq;
    using Moq.AutoMock;

    public class AutoMockerFixture : AutoMocker, IDisposable
    {
        private bool _disposedValue;

        public AutoMockerFixture()
            : base(MockBehavior.Strict)
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
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
