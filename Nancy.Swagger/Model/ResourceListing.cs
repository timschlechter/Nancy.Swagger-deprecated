using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Swagger.Model
{
    public class ResourceListing : SwaggerListing
    {
        /// <summary>
        /// Required. Lists the resources to be described by this specification implementation. The
        /// array can have 0 or more elements.
        /// </summary>
        [JsonProperty("apis", Required = Required.Always)]
        public IEnumerable<Resource> Apis { get; set; }

        /// <summary>
        /// Provides metadata about the API. The metadata can be used by the clients if needed, and
        /// can be presented in the Swagger-UI for convenience.
        /// </summary>
        [JsonProperty("info")]
        public Info Info { get; set; }
    }
}