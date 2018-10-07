using System;
using System.Collections.Generic;

namespace VendorService.Models.DbTableModels
{
    public class VendorInformationDbModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Rating { get; set; }
        public List<CategoryDbModel> CategoryModels { get; set; }
    }
}
