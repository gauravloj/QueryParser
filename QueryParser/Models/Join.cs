using Newtonsoft.Json;
using System.Collections.Generic;

namespace QueryParser.Models
{
    public class Join
    {
        [JsonProperty("join_type", Required = Required.Always)]
        public JoinType JoinType { get; set; }

        [JsonProperty("join_source", Required = Required.Always)]
        public Table JoinTableName { get; set; }

        [JsonProperty("join_condition", Required = Required.Always)]
        public JoinCondition JoinCondition { get; set; }
    }



}
