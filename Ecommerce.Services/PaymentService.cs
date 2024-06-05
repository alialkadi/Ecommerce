using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.Order;
using Ecommerce.Core.Repositories.Interfaces;
using Ecommerce.Core.Services.Interfaces;
using Ecommerce.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Ecommerce.Core.Entities.Product;

namespace Ecommerce.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly Core.Repositories.Interfaces.IGenericRepository<Product> _genericRepository;

		//Get Basket
		//Total Price
		//Call Stripe
		//REturn Basket included payment intentId and client secret

		public PaymentService( IBasketRepository basketRepository,
			IUnitOfWork unitOfWork, StoreDbContext context,
			IConfiguration configuration, IGenericRepository<Product> genericRepository)
        {
			this._basketRepository = basketRepository;
			this._unitOfWork = unitOfWork;
			this._configuration = configuration;
			this._genericRepository = genericRepository;
		}

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketid)
		{
			var basket = await _basketRepository.GetCustomerBasketAsync( basketid );
			if (basket == null) return null;

			if(basket.Items.Count > 0)
			{
				foreach( var item in basket.Items )
				{
					try
					{
						// Log the item ID
						Console.WriteLine($"Processing item with ID: {item.Id}");

                        var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);

                        // Log the retrieved product
                        Console.WriteLine($"Retrieved product: {product.Name}, Price: {product.Price}");

						if (item.Price != product.Price)
						{
							item.Price = product.Price;
						}
					}
					catch (Exception ex)
					{
						// Log the exception
						Console.WriteLine($"Error retrieving product with ID: {item.Id}, Exception: {ex.Message}");
					}
				}
			}
			var subTotalPrice = basket.Items.Sum( x => x.Price * x.Quantity );
			var shippingPrice = 0m;
			if(basket.DeliveryMethodId.HasValue)
			{
				var deliveryMethod= await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
				shippingPrice = deliveryMethod.Cost;
			}

			StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
			var service = new PaymentIntentService();
			PaymentIntent paymentIntent ;
			if(string.IsNullOrEmpty(basket.PaymentIntentId))
			{
				// Create new paymentintentid
				var options = new PaymentIntentCreateOptions()
				{
					Amount = (long)((subTotalPrice + shippingPrice) * 100),
					Currency = "usd",
					PaymentMethodTypes = new List<string>() { "card"}
					
				};
				paymentIntent = await service.CreateAsync(options);
				basket.PaymentIntentId = paymentIntent.Id;
				basket.clientSecret = paymentIntent.ClientSecret;
			}
			else
			{
				var options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)((subTotalPrice + shippingPrice) * 100)
				};
				paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, options);
				basket.PaymentIntentId = paymentIntent.Id;
				basket.clientSecret = paymentIntent.ClientSecret;
			}
			var OrderedBaskent = await _basketRepository.UpdateBasketAsync(basket);
			
			return OrderedBaskent;
		}
	}
}
