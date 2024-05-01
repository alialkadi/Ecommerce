﻿using Ecommerce.APIs.Errors;
using System.Net;
using System.Text.Json;

namespace Ecommerce.APIs.MiddleWares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleware( RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env )
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
			catch ( Exception ex )
			{
				_logger.LogError(ex, ex.Message);
				// Log Exception in Database [ in production ] 
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

				var res = _env.IsDevelopment() ?
						new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString())
						:new  ApiExceptionResponse(500);
				var response = JsonSerializer.Serialize(res);
				await context.Response.WriteAsync(response);

			}
		}  
    }
}
