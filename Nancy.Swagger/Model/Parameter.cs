using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nancy.Swagger.Model
{
    public class Parameter : DataType
    {
        /// <summary>
        /// Another way to allow multiple values for a "query" parameter. If used, the query
        /// parameter may accept comma-separated values. The field may be used only if paramType is
        /// "query", "header" or "path".
        /// </summary>
        [JsonProperty("allowMultiple")]
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// Recommended. A brief description of this parameter.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Required. The unique name for the parameter. Each name MUST be unique, even if they are
        /// associated with different paramType values. Parameter names are case sensitive.
        /// - If paramType is "path", the name field MUST correspond to the associated path segment
        ///   from the path field in the API Object.
        /// - If paramType is "query", the name field corresponds to the query parameter name.
        /// - If paramType is "body", the name is used only for Swagger-UI and Swagger-Codegen. In
        ///   this case, the name MUST be "body".
        /// - If paramType is "form", the name field corresponds to the form parameter key.
        /// - If paramType is "header", the name field corresponds to the header parameter key.
        /// - See here for some examples.
        /// </summary>
        [JsonProperty("name", Required = Newtonsoft.Json.Required.Always)]
        public string Name { get; set; }

        /// <summary>
        /// Required. The type of the parameter (that is, the location of the parameter in the
        /// request) . The value MUST be one of these values: "path", "query", "body", "header",
        /// "form". Note that the values MUST be lower case.
        /// </summary>
        [JsonProperty("paramType", Required = Newtonsoft.Json.Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParamType ParamType { get; set; }

        /// <summary>
        /// A flag to note whether this parameter is required. If this field is not included, it is
        /// equivalent to adding this field with the value false. The field MUST be included if
        /// paramType is "path" and MUST have the value true.
        /// </summary>
        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}