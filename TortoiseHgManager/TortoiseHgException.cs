using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TortoiseHgManager
{
    class TortoiseHgException : Exception
    {
        public TortoiseHgException(string message) : base(message) { }
        public TortoiseHgException(string message, Exception innerException) : base(message, innerException) { }
    }
}
