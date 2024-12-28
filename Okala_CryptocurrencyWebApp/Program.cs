using Microsoft.OpenApi.Models;
using OKala_CryptocurrencyApplication;
using Okala_CryptocurrencyWebApp.Midlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


//Application Layer
builder.Services.ConfigureApplicationServices(builder.Configuration);

// اضافه کردن Controllerها
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "Okala Exam Task Web Api"
    });
});




//Add Middelware Class That Implements IMiddleware Interface
builder.Services.AddScoped<SecureHeadersMiddleware>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // لاگ در کنسول
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}") // ذخیره لاگ‌ها در فایل
    .Enrich.FromLogContext()
    .CreateLogger();

// استفاده از Serilog به جای لاگ پیش‌فرض
builder.Host.UseSerilog();

// اضافه کردن سرویس Output Caching
builder.Services.AddOutputCache();



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
// استفاده از Output Caching Middleware
app.UseOutputCache();
// RequestTiming
app.UseMiddleware<RequestTimingMiddleware>();
app.MapControllers();

app.Run();
