using Newtonsoft.Json;
using System.Collections.Generic;

namespace QueryParser.Models
{
    public class Aggregation
    {
        [JsonProperty("groupby")]
        public List<Column> GroupByParameters { get; set; }

        [JsonProperty("group_filters")]
        public LogicalExpression HavingFilters { get; set; }
    }



}
