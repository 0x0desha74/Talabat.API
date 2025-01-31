using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.ProductBrand, O => O.MapFrom(s => s.ProductBrand.Name)) //in DTO we need the brand and type name not NP
                .ForMember(d => d.ProductType, O => O.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>()); //Resove Pictore
        }
    }
}
