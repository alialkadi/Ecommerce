using Ecommerce.APIs.Errors;
using Ecommerce.APIs.Helpers;
using Ecommerce.Core.Repositories.Interfaces;
using Ecommerce.Repository.Data;
using Ecommerce.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.APIs.Extentions
{
	public static class AddApplicationServices
	{
		public static IServiceCollection AddApplicationService(this IServiceCollection services)
		{
			// Add services to the container.

			

			
			//Services.AddScoped<IGenericRepository<Product>, GenericRepositories<Product>>();
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositories<>));

			//Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));
			services.AddAutoMapper(typeof(MappingProfile));
			services.Configure<ApiBehaviorOptions>(config =>
			{
				config.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
											.SelectMany(p => p.Value.Errors)
											.Select(p => p.ErrorMessage)
											.ToArray();

					var validationErrorResponse = new ApiValidationResponce()
					{
						Errors = errors
					};
					return new BadRequestObjectResult(validationErrorResponse);
				};
			});
			return services;
		}
	}
}
