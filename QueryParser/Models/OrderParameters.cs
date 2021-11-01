using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryParser.Models
{
    public class OrderParameters
    {
        [JsonProperty("ordering_columns")]
        public List<OrderingColumn> OrderingColumnList { get; set; }
    }
}
