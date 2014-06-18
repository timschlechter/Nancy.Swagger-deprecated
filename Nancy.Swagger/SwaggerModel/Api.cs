using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nancy.Swagger.Model
{
    public class Api : Resource
    {
		[JsonProperty("operations")]
        public IEnumerable<Operation> Operations { get; set; }
    }
}