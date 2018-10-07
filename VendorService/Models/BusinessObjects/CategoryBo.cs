using System;
using Newtonsoft.Json;

namespace VendorService.Models.BusinessObjects
{
    public class CategoryBo
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}