
namespace Ecommerce.APIs.Errors
{
	public class ApiResponse
	{
        public int StatusCode { get; set; }
        public string Message { get; set; }
		public List<string> Errors { get; set; } = null;

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMsgForStatusCode(statusCode);
            Errors = new List<string>();
        }

		private string? GetDefaultMsgForStatusCode(int statusCode)
		{
			return statusCode switch
			{
				200 => "OK",
				400 => "Bad Request has happened",
				401 => "UnAuthorized OUT ",
				404 => "Resource was not found ",
				409 => "Conflict Has occurred check the data",
				500 => "The BackEnd Developer is stupid as F, Server Error has occurred",
				_ => null
			};
		}
	}
}
