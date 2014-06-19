using Newtonsoft.Json;

namespace Nancy.Swagger.Model
{
    public class Resource
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }
    }
}