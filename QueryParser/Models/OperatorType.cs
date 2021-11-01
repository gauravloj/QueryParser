using System;
using System.Collections.Generic;
using System.Text;

namespace QueryParser.Models
{
    public enum OperatorType
    {
        EQUALS,
        NOT_EQUALS,
        GREATER_THAN,
        LESS_THAN,
        LESS_THAN_EQUALS,
        GREATER_THAN_EQUALS,
        IN,
        NOT_IN,
        LIKE
    }
}
