using Newtonsoft.Json;

namespace QueryParser.Models
{
    public class OrderingColumn
    {
        [JsonProperty("column_name")]
        public Column ColumnName { get; set; }

        [JsonProperty("sort_order")]
        public string SortOrder { get; set; }
    }

}
