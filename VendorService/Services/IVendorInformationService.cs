using System;
using VendorService.Models.BusinessObjects;

namespace VendorService.Services
{
    public interface IVendorInformationService
    {
        void DropVendorInformation();
        VendorInformationBo GetVendorInformation(Guid id);
        void InsertVendorInformation(VendorInformationBo vendorInfo);
    }
}