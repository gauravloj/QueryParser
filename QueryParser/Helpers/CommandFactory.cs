using QueryParser.Helpers.interfaces;
using QueryParser.Models;
using System;

namespace QueryParser.Helpers
{
    /// <summary>
    /// This factory returns Command parser based on sql query type
    /// </summary>
    class CommandFactory
    {
        public static ICommandParser GetCommandParser(QueryType type)
        {
            switch (type)
            {
                case QueryType.SELECT:
                    return new SelectCommandParser();
                default:
                    throw new NotImplementedException($"{type} Command not yet implemented.");
            }
        }
    }
}
