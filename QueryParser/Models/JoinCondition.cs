using Newtonsoft.Json;

namespace QueryParser.Models
{
    public class JoinCondition
    {

        [JsonProperty("left_expression")]
        public Column LeftExpression { get; set; }

        [JsonProperty("right_expression")]
        public Column RightExpression { get; set; }

        [JsonProperty("operator")]
        public string OperatorName { get; set; }

    }
}
