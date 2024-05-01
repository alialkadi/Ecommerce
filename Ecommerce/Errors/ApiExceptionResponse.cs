namespace Ecommerce.APIs.Errors
{
	public class ApiExceptionResponse : ApiResponse
	{
        public string? Details { get; set; }

        public ApiExceptionResponse(int code , string? msg = null, string? details =null ) : base(code, msg) 
        {
            Details = details;
        }
    }
}
