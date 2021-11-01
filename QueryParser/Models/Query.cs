using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryParser.Models
{
    public class Query
    {
        [JsonProperty("query_type", Required = Required.Always)]
        public QueryType CommandType { get; set; }

        [JsonProperty("query_parameters", Required = Required.Always)]
        public QueryParameters QueryParameters { get; set; }

    }
}
