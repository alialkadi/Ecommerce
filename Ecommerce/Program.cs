using Ecommerce.APIs.Errors;
using Ecommerce.APIs.Extentions;
using Ecommerce.APIs.Helpers;
using Ecommerce.APIs.MiddleWares;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.Repositories.Interfaces;
using Ecommerce.Repository.Data;
using Ecommerce.Repository.Identity;
using Ecommerce.Repository.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Ecommerce.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<StoreDbContext>(option =>
			{
				option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			}
			);
			builder.Services.AddDbContext<AppIdentityDbContext>(option =>
			{
				option.UseSqlServer(builder.Configuration.GetConnectionString("Identity"));
			}
			);
			builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
			{
				var connection = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});
			builder.Services.AddScoped<IBasketRepository, BasketRepository>();
			builder.Services.AddApplicationService();
			builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
			{

			}).AddEntityFrameworkStores<AppIdentityDbContext>();
			#endregion



			var app = builder.Build();

			using var scope = app.Services.CreateScope();
			var service = scope.ServiceProvider;
			var _context = service.GetRequiredService<StoreDbContext>();
			var _IdentityContext = service.GetRequiredService<AppIdentityDbContext>();

			var loggerFactory = service.GetRequiredService<ILoggerFactory>();
			try
			{
				await _context.Database.MigrateAsync();
				await StoreDbContextSeed.SeedAsync(_context);

				await _IdentityContext.Database.MigrateAsync();
				var userManager = service.GetRequiredService<UserManager<AppUser>>();
				await AppIdentityDbContextSeed.SeedUserAsync(userManager);
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError( ex, ex.Message + " \n During the migration" );
			}

			// Configure the HTTP request pipeline.
			app.UseMiddleware<ExceptionMiddleware>();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStatusCodePagesWithReExecute("/error/{0}");
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
