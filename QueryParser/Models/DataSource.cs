using Newtonsoft.Json;
using System.Collections.Generic;

namespace QueryParser.Models
{
    public class DataSource
    {
        [JsonProperty("source_table", Required = Required.Always)]
        public Table SourceTableName { get; set; }
        
        [JsonProperty("joins")]
        public List<Join> JoinsList { get; set; }
    }



}
