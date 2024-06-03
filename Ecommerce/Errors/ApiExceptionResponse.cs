namespace Ecommerce.APIs.Errors
{
	public class ApiExceptionResponse
	{
		public string Message { get; }
		public string Details { get; }
		public int StatusCode { get; }

		public ApiExceptionResponse(int statusCode, string message = null, string details = null)
		{
			StatusCode = statusCode;
			Message = message ?? "An error occurred.";
			Details = details;
		}
	}
}
