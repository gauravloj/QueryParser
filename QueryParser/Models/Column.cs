using Newtonsoft.Json;

namespace QueryParser.Models
{
    public class Column
    {
        [JsonProperty("parent_table")]
        public string ParentTableName { get; set; }

        [JsonProperty("column_name", Required = Required.Always)]
        public string ColumnName { get; set; }

        [JsonProperty("column_type", Required = Required.Always)]
        public ColumnTypes ColumnType { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }
    }



}
