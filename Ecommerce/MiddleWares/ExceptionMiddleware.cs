using Ecommerce.APIs.Errors;
using System.Net;
using System.Text.Json;

namespace Ecommerce.APIs.MiddleWares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				ApiExceptionResponse res;
				if (_env.IsDevelopment())
				{
					res = new ApiExceptionResponse(500, ex.Message, ex.StackTrace?.ToString());
				}
				else
				{
					res = new ApiExceptionResponse(500);
				}

				try
				{
                    await Console.Out.WriteLineAsync("Tgis  is a messge from middleware");
                    var response = JsonSerializer.Serialize(res);
					await context.Response.WriteAsync(response);
				}
				catch (Exception jsonEx)
				{
					_logger.LogError(jsonEx, "Error serializing the exception response");

					// Fallback response in case of serialization error
					await context.Response.WriteAsync("{\"statusCode\": 500, \"message\": \"An error occurred.\"}");
				}
			}
		}
	}
}
