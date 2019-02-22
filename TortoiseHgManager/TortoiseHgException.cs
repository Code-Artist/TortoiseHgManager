using System;

namespace TortoiseHgManager
{
    class TortoiseHgException : Exception
    {
        public TortoiseHgException(string message) : base(message) { }
        public TortoiseHgException(string message, Exception innerException) : base(message, innerException) { }
    }
}
