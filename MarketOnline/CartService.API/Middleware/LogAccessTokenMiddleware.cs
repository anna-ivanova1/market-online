namespace CartService.API.Middleware
{
	/// <summary>
	/// Log access token middleware
	/// </summary>
	public class LogAccessTokenMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<LogAccessTokenMiddleware> _logger;

		/// <summary>
		/// Initialize a new instance of the <see cref="LogAccessTokenMiddleware"/> class.
		/// </summary>
		/// <param name="next">RequestDelegate</param>
		/// <param name="logger">Logger</param>
		public LogAccessTokenMiddleware(RequestDelegate next, ILogger<LogAccessTokenMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		/// <summary>
		/// Invoke Async
		/// </summary>
		/// <param name="context">HttpContext</param>
		/// <returns>A task</returns>
		public async Task InvokeAsync(HttpContext context)
		{
			if (context.User.Identity?.IsAuthenticated == true)
			{
				var userName = context.User.FindFirst("preferred_username")?.Value;

				_logger.LogInformation("Access token details: userName={userName}", userName);
			}

			await _next(context);
		}
	}
}
