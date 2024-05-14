namespace Ecommerce.APIs.Dtos
{
	public class ForgetPassWordResponse
	{
        public string Token { get; set; }
        public string Email { get; set; }
        public int ResetCode { get; set; }
        public string Url { get; set; }


    }
}
