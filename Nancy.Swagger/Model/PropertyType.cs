using Newtonsoft.Json;

namespace Nancy.Swagger.Model
{
    public class PropertyType : DataType
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}