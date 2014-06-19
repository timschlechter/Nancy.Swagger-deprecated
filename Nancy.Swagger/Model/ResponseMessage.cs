using Newtonsoft.Json;

namespace Nancy.Swagger.Model
{
    public class Responsemessage
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}