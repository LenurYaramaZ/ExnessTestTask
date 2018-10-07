using AutoMapper;
using VendorService.Models.BusinessObjects;
using VendorService.Models.DbTableModels;

namespace VendorService.Helper
{
    public static class VendorInformationHelper
    {
        public static VendorInformationBo ToVendorInformationBo(this VendorInformationDbModel vendorInformationDbModel)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VendorInformationDbModel, VendorInformationBo>()
                    .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.CategoryModels));
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<VendorInformationDbModel, VendorInformationBo>(vendorInformationDbModel);
        }

        public static VendorInformationDbModel ToVendorInformationDbModel(this VendorInformationBo vendorInformationBo)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VendorInformationBo, VendorInformationDbModel>()
                    .ForMember(dest => dest.CategoryModels, opt => opt.MapFrom(src => src.Categories));
            });
            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<VendorInformationBo, VendorInformationDbModel>(vendorInformationBo);
        }
    }
}