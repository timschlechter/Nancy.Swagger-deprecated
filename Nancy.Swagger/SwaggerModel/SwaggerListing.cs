using Newtonsoft.Json;

namespace Nancy.Swagger.Model
{
    public abstract class SwaggerListing
    {
        /// <summary>
        /// Provides the version of the application API (not to be confused by the specification version).
        /// </summary>
        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }

        /// <summary>
        /// Provides information about the the authorization schemes allowed on his API.
        /// </summary>
        [JsonProperty("authorizations")]
        public Authorizations Authorizations { get; set; }

        /// <summary>
        /// Required. Specifies the Swagger Specification version being used. It can be used by the
        /// Swagger UI and other clients to interpret the API listing. The value MUST be an existing
        /// Swagger specification version. Currently, "1.0", "1.1", "1.2" are valid values. The
        /// field is of string value for possible non-numeric versions in the future (for example, "1.2a").
        /// </summary>
        [JsonProperty("swaggerVersion", Required = Required.Always)]
        public string SwaggerVersion { get; set; }
    }
}