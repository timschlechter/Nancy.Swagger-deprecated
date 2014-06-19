using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Swagger.Model
{
    public abstract class DataType
    {
        /// <summary>
        /// The default value to be used for the field. The value type MUST conform with the
        /// primitive's type value.
        /// </summary>
        [JsonProperty("defaultValue")]
        public object DefaultValue { get; set; }

        /// <summary>
        /// A fixed list of possible values. If this field is used in conjunction with the
        /// defaultValue field, then the default value MUST be one of the values defined in the enum.
        /// </summary>
        [JsonProperty("enum")]
        public IEnumerable<string> Enum { get; set; }

        /// <summary>
        /// Fine-tuned primitive type definition. See Primitives for further information. The value
        /// MUST be one that is defined under Primitives, corresponding to the right primitive type.
        /// </summary>
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// Required. The type definition of the values in the container. A container MUST NOT be
        /// nested in another container.
        /// </summary>
        [JsonProperty("items", Required = Required.Always)]
        public Items Items { get; set; }

        /// <summary>
        /// The maximum valid value for the type, inclusive. If this field is used in conjunction
        /// with the defaultValue field, then the default value MUST be lower than or equal to this
        /// value. The value type is string and should represent the maximum numeric value. Note:
        /// This will change to a numeric value in the future.
        /// </summary>
        [JsonProperty("maximum")]
        public long? Maximum { get; set; }

        /// <summary>
        /// The minimum valid value for the type, inclusive. If this field is used in conjunction
        /// with the defaultValue field, then the default value MUST be higher than or equal to this
        /// value. The value type is string and should represent the minimum numeric value. Note:
        /// This will change to a numeric value in the future.
        /// </summary>
        [JsonProperty("minimum")]
        public long? Minimum { get; set; }

        /// <summary>
        /// Required (if type is not used). The Model to be used. The value MUST be a model's id.
        /// </summary>
        [JsonProperty("$ref")]
        public string Ref { get; set; }

        /// <summary>
        /// Required (if $ref is not used). The return type of the operation. The value MUST be one
        /// of the Primitves, array or a model's id.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// A flag to note whether the container allows duplicate values or not. If the value is set
        /// to true, then the array acts as a set.
        /// </summary>
        [JsonProperty("uniqueItems")]
        public bool? UniqueItems { get; set; }
    }
}