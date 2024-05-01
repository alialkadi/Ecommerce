namespace Ecommerce.APIs.Errors
{
	public class ApiValidationResponce : ApiResponse
	{
		public IEnumerable<string> Errors { get; set; }

        public ApiValidationResponce() : base(400)
        {
            
        }
    }
}
