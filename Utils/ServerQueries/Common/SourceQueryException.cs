using System;

namespace ServerQueries.Common
{
    public class SourceQueryException : Exception
    {
        public SourceQueryException() : base() { }
        public SourceQueryException(string message) : base(message) { }
        public SourceQueryException(string message, Exception innerException) : base(message, innerException) { }
    }
}