using System;
using VendorService.Models.DbTableModels;

namespace VendorService.Repository
{
    internal interface IVendorInformationRepository
    {
        void DropDb();
        VendorInformationDbModel GetVendorInformation(Guid id);
        void InsertVendorInformation(VendorInformationDbModel vendorInfo);
    }
}