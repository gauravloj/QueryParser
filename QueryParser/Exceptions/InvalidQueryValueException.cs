using System;
using System.Collections.Generic;
using System.Text;

namespace QueryParser.Exceptions
{
    class InvalidQueryValueException : Exception
    {
        public InvalidQueryValueException(string message): base( message) { }
    }
}
