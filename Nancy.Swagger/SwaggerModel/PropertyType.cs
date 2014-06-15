using Newtonsoft.Json;

namespace Nancy.Swagger.Model
{
    public class PropertyType
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("maximum")]
        public string Maximum { get; set; }

        [JsonProperty("minimum")]
        public string Minimum { get; set; }

        [JsonProperty("$ref")]
        public string Reference { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        internal string Name { get; set; }
    }
}