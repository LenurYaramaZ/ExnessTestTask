using System;
using Newtonsoft.Json;

namespace AutoTestApp.Models.Api.GetVendorInformation.Response
{
    public class Category
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}