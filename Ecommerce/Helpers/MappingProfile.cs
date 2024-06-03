using AutoMapper;
using Ecommerce.APIs.Dtos;
using Ecommerce.Core.Entities;
using UserAddress = Ecommerce.Core.Entities.Identity.Address;
using OrderAddress = Ecommerce.Core.Entities.Order.Address;
using Ecommerce.Core.Entities.Order;

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

            CreateMap<UserAddress, AddressDto>().ReverseMap();
            CreateMap<OrderAddress, AddressDto>().ReverseMap();
            CreateMap<Order, OrderResponseDto>()
                .ForMember(D=> D.deliveryMethod, O=> O.MapFrom(S=>S.deliveryMethod.ShortName))
                .ForMember(D=> D.DeliveryMethodCost, O=> O.MapFrom(S=>S.deliveryMethod.Cost));

			CreateMap<OrderItem, OrderItemDto>()
				.ForMember(D => D.ProductId, O => O.MapFrom(S => S.Product.ProductId))
				.ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
				.ForMember(D => D.PictureUrl, O => O.MapFrom<OrderItemPictureREsolver>());

            CreateMap<CustomerBasket,CustomerBasketDTO>().ReverseMap();
		}
    }
}
