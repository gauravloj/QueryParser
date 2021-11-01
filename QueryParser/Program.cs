using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QueryParser.Helpers;
using QueryParser.Helpers.interfaces;
using QueryParser.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace QueryParser
{
    class Program
    {
        private const int ARGS_COUNT = 1;
        static void Main(string[] args)
        {

            // Command : dotnet run -- C:\gitRepo\QueryParser\QueryParserUnitTest\sqlquery.json
            if (args.Length < ARGS_COUNT)
            {
                throw new ArgumentException("Expected input file path as argument");
            }

            try
            {
                string input = File.ReadAllText(args[0]);
                List<Query> queryList = JsonConvert.DeserializeObject<List<Query>>(input);

                foreach (var query in  queryList)
                {
                    ICommandParser parser = CommandFactory.GetCommandParser(query.CommandType);

                    Console.WriteLine(parser.Parse(query));
                    Console.WriteLine("\n ----------------------------------------------------- \n");
                }

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"Invalid file path: {e.Message}");
            }
            catch (JsonSerializationException e)
            {
                Console.WriteLine($"Invalid JSON input: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unknown exception: {e.GetType()}, {e.Message}");
            }
        }
    }
}
