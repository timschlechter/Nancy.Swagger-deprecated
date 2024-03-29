﻿using System.Collections.Generic;
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

		[JsonProperty("tags", Required = Required.AllowNull)]
		public IEnumerable<string> Tags { get; set; }

		[JsonProperty("friends", Required = Required.AllowNull)]
		public Person[] Friends { get; set; }
    }
}