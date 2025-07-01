namespace CartService.API.Middleware
{
    public class LogAccessTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogAccessTokenMiddleware> _logger;

        public LogAccessTokenMiddleware(RequestDelegate next, ILogger<LogAccessTokenMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

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
