using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Swagger.Model
{
    public class Model
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, PropertyType> Properties { get; set; }

        [JsonProperty("required")]
        public string[] Required { get; set; }
    }
}