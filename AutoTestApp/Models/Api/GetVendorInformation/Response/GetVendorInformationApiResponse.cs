using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AutoTestApp.Models.Api.GetVendorInformation.Response
{
    public class GetVendorInformationApiResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rating")]
        public int? Rating { get; set; }

        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }
    }
}