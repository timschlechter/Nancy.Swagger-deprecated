using Newtonsoft.Json;

namespace Nancy.Swagger.Model
{
    public class Items
    {
        [JsonProperty("$ref")]
        public string Ref { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}