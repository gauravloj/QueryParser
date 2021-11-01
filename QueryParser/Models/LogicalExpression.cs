using Newtonsoft.Json;

namespace QueryParser.Models
{
    public class LogicalExpression
    {
        [JsonProperty("left_expression", Required = Required.Always)]
        public FilterCondition LeftExpression { get; set; }

        [JsonProperty("operator")]
        public LogicalOperatorTypes OperatorName { get; set; }

        [JsonProperty("right_expression")]
        public LogicalExpression RightExpression { get; set; }

    }



}
