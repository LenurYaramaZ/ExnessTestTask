using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VendorService.Models.BusinessObjects
{
    public class VendorInformationBo
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rating")]
        public int? Rating { get; set; }

        [JsonProperty("categories")]
        public List<CategoryBo> Categories { get; set; }
    }
}