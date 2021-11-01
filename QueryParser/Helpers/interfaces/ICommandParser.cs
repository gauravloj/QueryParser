using QueryParser.Models;

namespace QueryParser.Helpers.interfaces
{
    /// <summary>
    /// This interface provides prototype for a command parser  for sql commands
    /// </summary>
    public interface ICommandParser
    {
        string Parse(Query query);
    }
}
