using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Swagger.Model
{
    public class Operation : DataType
    {
        [JsonProperty("authorizations")]
        public Authorizations Authorizations { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("parameters")]
        public IEnumerable<Parameter> Parameters { get; set; }

        [JsonProperty("responseMessages")]
        public IEnumerable<Responsemessage> ResponseMessages { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

    }
}