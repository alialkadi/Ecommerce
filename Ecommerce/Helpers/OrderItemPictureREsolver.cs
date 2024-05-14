using AutoMapper;
using Ecommerce.APIs.Dtos;
using Ecommerce.Core.Entities.Order;

namespace Ecommerce.APIs.Helpers
{
	public class OrderItemPictureREsolver : IValueResolver<OrderItem, OrderItemDto, string>
	{
		private readonly IConfiguration configuration;

		public OrderItemPictureREsolver(IConfiguration configuration)
        {
			this.configuration = configuration;
		}
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
		{
			if (string.IsNullOrEmpty(source.Product.PictureUrl))
			{
				return string.Empty;
			}
			else
			{
				return $"{configuration["BaseUrl"]}/{source.Product.PictureUrl}";
			}
		}
	}
}
