using Newtonsoft.Json;
namespace Nancy.Swagger.TestApp.Model
{
    public class Person
    {
        [JsonProperty("age", Required = Required.Always)]
        public int Age { get; set; }

        [JsonProperty("name", Required = Required.Default)]
        public string Name { get; set; }

        [JsonProperty("address", Required = Required.AllowNull)]
        public Address Address { get; set; }
    }
}