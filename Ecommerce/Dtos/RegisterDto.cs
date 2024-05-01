using System.ComponentModel.DataAnnotations;

namespace Ecommerce.APIs.Dtos
{
	public class RegisterDto
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Form")]
		public string Email { get; set; }
		[Required(ErrorMessage ="User Name is required")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[MinLength(6, ErrorMessage = "Minimum length is 6 chars and maximum length is 10")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%&*()_+]).{6,10}$", 
			ErrorMessage = "Password must contain 1 uppercase, 1 lowercase, 1 digit, 1 special character, and be 6 to 10 characters long.")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare(nameof(Password), ErrorMessage = "Password does not match")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%&*()_+]).{6,10}$", 
			ErrorMessage = "Password must contain 1 uppercase, 1 lowercase, 1 digit, 1 special character, and be 6 to 10 characters long.")]

		public string rePassword { get; set; }
		public string Phone { get; set; }

	}
}
