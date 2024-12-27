using Serilog;
using System.Diagnostics;

namespace Okala_CryptocurrencyWebApp.Midlewares;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // ثبت زمان شروع
        var startTime = DateTime.UtcNow;
        context.Items["RequestStartTime"] = startTime;

        // ادامه درخواست به Middleware یا کنترلر بعدی
        await _next(context);

        // ثبت زمان پایان
        stopwatch.Stop();
        var duration = stopwatch.ElapsedMilliseconds;

        // لاگ کردن یا ذخیره اطلاعات
        var endTime = DateTime.UtcNow;
        Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
        Console.WriteLine($"Start Time: {startTime}");
        Console.WriteLine($"End Time: {endTime}");
        Console.WriteLine($"Duration: {duration} ms");

        // ذخیره اطلاعات در لاگ‌ها
        Log.Information("Request: {Method} {Path} | Start: {StartTime} | End: {EndTime} | Duration: {Duration}ms",
            context.Request.Method,
            context.Request.Path,
            startTime,
            endTime,
            duration);

    }
}