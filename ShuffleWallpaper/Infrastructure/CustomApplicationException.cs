namespace ShuffleWallpaper.Infrastructure
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]

    public class CustomApplicationException : ApplicationException
    {
        public CustomApplicationException()
        {
        }

        public CustomApplicationException(string message)
            : base(message)
        {
        }

        protected CustomApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
