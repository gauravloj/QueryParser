using QueryParser.Helpers.interfaces;
using QueryParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryParser.Helpers
{
    /// <summary>
    /// This calss implements command parser for Select command
    /// It validates the sequence of clauses for select query
    /// </summary>
    public class SelectCommandParser : ICommandParser
    {
        public string Parse(Query query)
        {
            if (query.CommandType != QueryType.SELECT)
            {
                throw new ArgumentException($"Invalid Query Type {query.CommandType}. Expected query type is {QueryType.SELECT}");

            }

            StringBuilder queryBuilder = new StringBuilder();

            queryBuilder.Append("SELECT ");

            queryBuilder.Append($"{ParserUtility.ParseColumns(query.QueryParameters.DisplayColumnList)}\n");

            if (query.QueryParameters.DataSources == null)
            {
                return queryBuilder.ToString();
            }
            
            queryBuilder.Append(ParserUtility.ParseDataSource(query.QueryParameters.DataSources));

            if (query.QueryParameters.FiltersList != null)
            {
                queryBuilder.Append("WHERE ");
                queryBuilder.Append($"{ParserUtility.ParseFilters(query.QueryParameters.FiltersList)}\n");
            }

            if (query.QueryParameters.AggregationParameters != null)
            {
                queryBuilder.Append("GROUP BY ");
                queryBuilder.Append($"{ParserUtility.ParseAggregation(query.QueryParameters.AggregationParameters)}\n");
            }

            if (query.QueryParameters.OrderingParameters != null)
            {
                queryBuilder.Append("Order BY ");
                queryBuilder.Append(ParserUtility.ParseOrderingParams(query.QueryParameters.OrderingParameters));
            }



            return queryBuilder.ToString();

        }
    }
}
