using System.ComponentModel.DataAnnotations;

namespace Ecommerce.APIs.Dtos
{
	public class OrderDto
	{
        [Required(ErrorMessage ="BasketId is Required")]
        public string BasketId { get; set; }
		[Required(ErrorMessage ="Delivery Method is required is Required")]
        public int DeliveryMethodId { get; set; }
        [Required(ErrorMessage ="Address is Required")]
		public AddressDto Address { get; set; }

    }
}
