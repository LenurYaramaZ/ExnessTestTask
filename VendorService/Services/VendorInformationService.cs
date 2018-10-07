using System;
using System.Configuration;
using VendorService.Helper;
using VendorService.Models.BusinessObjects;
using VendorService.Repository;

namespace VendorService.Services
{
    public class VendorInformationService : IVendorInformationService
    {
        private IVendorInformationRepository _repository;

        public void DropVendorInformation()
        {
            VendorInformationRepository.DropDb();
        }

        public void InsertVendorInformation(VendorInformationBo vendorInfo)
        {
            VendorInformationRepository.InsertVendorInformation(vendorInfo.ToVendorInformationDbModel());
        }

        public VendorInformationBo GetVendorInformation(Guid id)
        {
            return VendorInformationRepository.GetVendorInformation(id).ToVendorInformationBo();
        }

        private IVendorInformationRepository VendorInformationRepository
            => _repository ?? (_repository = new VendorInformationRepository
                   (ConfigurationManager.ConnectionStrings["LiteDB"].ConnectionString));
    }
}