using Newtonsoft.Json;

namespace Nancy.Swagger.Model
{
    public class Info
    {
        [JsonProperty("contact")]
        public string Contact { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("license")]
        public string License { get; set; }

        [JsonProperty("licenseUrl")]
        public string LicenseUrl { get; set; }

        [JsonProperty("termsOfServiceUrl")]
        public string TermsOfServiceUrl { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}