using QueryParser.Exceptions;
using QueryParser.Helpers.interfaces;
using QueryParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryParser.Helpers
{
    /// <summary>
    /// Utility class to generate sql query for different clauses
    /// </summary>
    class ParserUtility
    {
        /// <summary>
        /// Convert column object into sql query string
        /// </summary>
        /// <param name="columns"></param>
        /// <returns>sql string for list of columns</returns>
        public static string ParseColumns(List<Column> columns)
        {
            if (columns == null)
            {
                return "* ";
            }

            StringBuilder columnBuilder = new StringBuilder();

            for (int i = 0; i<columns.Count; i++)
            {
                columnBuilder.Append(ParseColumn(columns[i]));
                if (i < columns.Count - 1)
                {
                    columnBuilder.Append(", ");
                }

            }
            return columnBuilder.ToString();
        }

        /// <summary>
        /// Converts a single column object to column string
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static string ParseColumn(Column column)
        {
            StringBuilder columnBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(column.ColumnName))
            {
                throw new InvalidQueryValueException("Column Name Empty");
            }

            if (!string.IsNullOrEmpty(column.ParentTableName))
            {
                columnBuilder.Append($"\"{column.ParentTableName}\".");
            }

            switch (column.ColumnType)
            {
                case ColumnTypes.STRING:
                    columnBuilder.Append($"\"{column.ColumnName}\" ");
                    break;
                case ColumnTypes.EXPRESSION:
                case ColumnTypes.NAME:
                    columnBuilder.Append($"{column.ColumnName} ");
                    break;
                default:
                    throw new InvalidQueryValueException("Invalid Column Type");
            }

            if (!string.IsNullOrEmpty(column.Alias))
            {
                columnBuilder.Append($"AS \"{column.Alias}\"");
            }

            return columnBuilder.ToString();
        }

        /// <summary>
        /// Converts a data source object into sql query string for from clause and joins
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>

        public static string ParseDataSource(DataSource source)
        {
            StringBuilder sourceBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(source.SourceTableName?.TableName))
            {
                throw new InvalidQueryValueException("Empty table name");
            }
            sourceBuilder.Append($"FROM {source.SourceTableName}\n");

            if (source.JoinsList?.Count > 0)
            {
                foreach (var join in source.JoinsList)
                {
                    sourceBuilder.Append(ParseJoin(join));
                }
            }

            return sourceBuilder.ToString();
        }

        /// <summary>
        /// Parses a single join clause
        /// </summary>
        /// <param name="join"></param>
        /// <returns></returns>
        public static string ParseJoin(Join join)
        {
            StringBuilder sourceBuilder = new StringBuilder();
            switch (join.JoinType)
            {
                case JoinType.LEFT_JOIN:
                    sourceBuilder.Append("LEFT JOIN ");
                    break;
                case JoinType.RIGHT_JOIN:
                    sourceBuilder.Append("RIGHT JOIN ");
                    break;
                case JoinType.FULL_JOIN:
                    sourceBuilder.Append("FULL JOIN ");
                    break;
                case JoinType.INNER_JOIN:
                    sourceBuilder.Append("JOIN ");
                    break;
                default:
                    break;
            }

            sourceBuilder.Append($"{join.JoinTableName} ON ");
            sourceBuilder.Append($"{ParseCondition(join.JoinCondition)}\n");

            return sourceBuilder.ToString();
        }

        /// <summary>
        /// Parses a single join condition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string ParseCondition(JoinCondition condition)
        {
            StringBuilder conditionBuilder = new StringBuilder();

            conditionBuilder.Append($"{ParseColumn(condition.LeftExpression)} = {ParseColumn(condition.RightExpression)}");

            return conditionBuilder.ToString();
        }

        /// <summary>
        /// It parses the ordering parameters to order by clause
        /// </summary>
        /// <param name="orderVals"></param>
        /// <returns></returns>
        public static string ParseOrderingParams(OrderParameters orderVals)
        {
            StringBuilder orderBuilder = new StringBuilder();


            for (int i = 0; i < orderVals.OrderingColumnList.Count; i++)
            {
                orderBuilder.Append($"{ParseColumn(orderVals.OrderingColumnList[i].ColumnName)} ");
                if (orderVals.OrderingColumnList[i].SortOrder != null)
                {
                    orderBuilder.Append($"{orderVals.OrderingColumnList[i].SortOrder.ToUpper()} ");
                }

                if (i < orderVals.OrderingColumnList.Count - 1)
                {
                    orderBuilder.Append(", ");
                }
            }


            return orderBuilder.ToString();
        }

        /// <summary>
        /// It parses aggregation parameters to generate "group by" and "having" clause
        /// </summary>
        /// <param name="aggregationParams"></param>
        /// <returns></returns>
        public static string ParseAggregation(Aggregation aggregationParams)
        {
            StringBuilder aggregateBuilder = new StringBuilder();

            if (aggregationParams.GroupByParameters == null)
            {
                throw new InvalidQueryValueException("Invalid group by columns");
            }

            aggregateBuilder.Append($" {ParseColumns(aggregationParams.GroupByParameters)} \n");
            
            if (aggregationParams.HavingFilters != null)
            {
                aggregateBuilder.Append($" HAVING {ParseFilters(aggregationParams.HavingFilters)} ");
            }

            return aggregateBuilder.ToString();
        }
        
        /// <summary>
        /// It parses logical expressions for where clause and having clause
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static string ParseFilters(LogicalExpression filters)
        {
            StringBuilder conditionBuilder = new StringBuilder();

            string left_expression = ParseFilter(filters.LeftExpression);

            if (!string.IsNullOrEmpty(left_expression))
            {
                conditionBuilder.Append($"( { left_expression}) ");
            }


            if(filters.RightExpression != null)
            {
                conditionBuilder.Append($"{filters.OperatorName}  {ParseFilters(filters.RightExpression)} ");
            }               

            return conditionBuilder.ToString();
        }

        /// <summary>
        /// It parses a single filter condition in logical expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string ParseFilter(FilterCondition filter)
        {
            StringBuilder conditionBuilder = new StringBuilder();

            conditionBuilder.Append($"{ParseColumn(filter.Field)} ");
            switch (filter.OperatorName)
            {
                case OperatorType.EQUALS:
                    conditionBuilder.Append("= ");
                    break;
                case OperatorType.NOT_EQUALS:
                    conditionBuilder.Append("!= ");
                    break;
                case OperatorType.GREATER_THAN:
                    conditionBuilder.Append("> ");
                    break;
                case OperatorType.LESS_THAN:
                    conditionBuilder.Append("< ");
                    break;
                case OperatorType.LESS_THAN_EQUALS:
                    conditionBuilder.Append("<= ");
                    break;
                case OperatorType.GREATER_THAN_EQUALS:
                    conditionBuilder.Append(">= ");
                    break;
                case OperatorType.IN:
                    conditionBuilder.Append("IN ");
                    break;
                case OperatorType.NOT_IN:
                    conditionBuilder.Append("NOT IN ");
                    break;
                case OperatorType.LIKE:
                    conditionBuilder.Append("LIKE ");
                    break;
                default:
                    throw new InvalidQueryValueException("Invalid Operator type");
            }

            switch (filter.ValueType)
            {
                case ValueTypes.LITERAL_STRING:
                    conditionBuilder.Append($"\"{filter.ValueLiteral}\" ");
                    break;
                case ValueTypes.LITERAL_NUMBER:
                    conditionBuilder.Append($"{filter.ValueLiteral} ");
                    break;
                case ValueTypes.QUERY:
                    conditionBuilder.Append($" ( {CommandFactory.GetCommandParser(filter.ValueQuery.CommandType).Parse(filter.ValueQuery)} )");
                    break;
                default:
                    break;
            }

            return conditionBuilder.ToString();
        }

    }
}
