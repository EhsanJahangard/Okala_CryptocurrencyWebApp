using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OKala_CryptocurrencyApplication.Configurations;
using OKala_CryptocurrencyApplication.Contracts;
using OKala_CryptocurrencyApplication.Services;

namespace OKala_CryptocurrencyApplication;
public static class AppicationServiceRegistration
{
    public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // اضافه کردن تنظیمات
        services.Configure<CryptoCurrencyApiSettings>(configuration.GetSection("ApiSettings"));

        // اضافه کردن HttpClient
        services.AddHttpClient<ICryptocurrencyRepository, CryptocurrencyRepository>();
        services.AddHttpContextAccessor();
        #region FluentValidation
        services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblyContaining<CryptoCurrentRequestDtoValidator>();
        #endregion

    }
}