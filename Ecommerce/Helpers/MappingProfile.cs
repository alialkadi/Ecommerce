using AutoMapper;
using Ecommerce.APIs.Dtos;
using Ecommerce.Core.Entities;

namespace Ecommerce.APIs.Helpers
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(D => D.Brand, O => O.MapFrom(s => s.Brand.Name))
                .ForMember(D => D.Category, O => O.MapFrom(s => s.Category.Name))
                .ForMember(D => D.PictureUrl, o => o.MapFrom<ProductPictureResolver>());
                
        }
    }
}
