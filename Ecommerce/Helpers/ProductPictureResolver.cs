using AutoMapper;
using Ecommerce.APIs.Dtos;
using Ecommerce.Core.Entities;

namespace Ecommerce.APIs.Helpers
{
	public class ProductPictureResolver : IValueResolver<Product, ProductToReturnDto, string>
	{
		private readonly IConfiguration configuration;

		public ProductPictureResolver(IConfiguration configuration)
        {
			this.configuration = configuration;
		}
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
		{
			if(string.IsNullOrEmpty(source.PictureUrl))
			{
				return string.Empty;
			}
			else
			{
				return $"{configuration["BaseUrl"]}/{source.PictureUrl}";
			}
		}
	}
}
