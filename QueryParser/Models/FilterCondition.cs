using Newtonsoft.Json;

namespace QueryParser.Models
{
    public class FilterCondition
    {

        [JsonProperty("field", Required = Required.Always)]
        public Column Field { get; set; }

        [JsonProperty("value_literal")]
        public string ValueLiteral { get; set; }

        [JsonProperty("value_object")]
        public Query ValueQuery { get; set; }

        [JsonProperty("value_type", Required = Required.Always)]
        public ValueTypes ValueType { get; set; }

        [JsonProperty("operator", Required = Required.Always)]
        public OperatorType OperatorName { get; set; }
    }



}
