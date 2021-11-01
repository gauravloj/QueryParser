using Newtonsoft.Json;

namespace QueryParser.Models
{
    public class Table
    {

        [JsonProperty("table_name", Required = Required.Always)]
        public string TableName { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        public override string ToString() {
            string tablename = this.TableName;
            
            if (!string.IsNullOrEmpty(this.Alias))
            {
                tablename += $" AS {this.Alias}";
            }
            
            return tablename;
        }
    }
}
