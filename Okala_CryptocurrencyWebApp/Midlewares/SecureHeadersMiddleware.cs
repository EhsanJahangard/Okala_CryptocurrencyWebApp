namespace Okala_CryptocurrencyWebApp.Midlewares;
/// <summary>
/// Hardenning WebApp
/// </summary>
public class SecureHeadersMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "no-referrer");
        context.Response.Headers.Append("Content-Security-Policy", "default-src 'self';");
        context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        context.Response.Headers.Remove("X-Powered-By");
        context.Response.Headers.Remove("x-aspnet-version");
        await next(context);
    }
}
