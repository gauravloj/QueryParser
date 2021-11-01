using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryParser.Models
{
    public class QueryParameters
    {
        [JsonProperty("display_columns")]
        public List<Column> DisplayColumnList { get; set; }

        [JsonProperty("data_source")]
        public DataSource DataSources { get; set; }

        [JsonProperty("filter")]
        public LogicalExpression FiltersList { get; set; }

        [JsonProperty("aggregation")]
        public Aggregation AggregationParameters { get; set; }

        [JsonProperty("order_parameters")]
        public OrderParameters OrderingParameters { get; set; }
    }


}
