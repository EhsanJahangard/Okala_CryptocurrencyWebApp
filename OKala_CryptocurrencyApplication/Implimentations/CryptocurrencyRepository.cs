#region using
using Microsoft.Extensions.Options;
using OKala_CryptocurrencyApplication.Configurations;
using OKala_CryptocurrencyApplication.Contracts;
using OKala_CryptocurrencyDomain.Dtos.RequestDto;
using OKala_CryptocurrencyDomain.Dtos.ResponseDto;
using OKala_CryptocurrencyDomain.Models;
using OKala_CryptocurrencyInfrastructure.Utilities;
using Polly;
using Polly.Retry;
using Serilog;
using System.Net.Http.Headers;
using System.Text.Json;
#endregion

namespace OKala_CryptocurrencyApplication.Services;

public class CryptocurrencyRepository : ICryptocurrencyRepository
{

    #region private_Fields
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _endpoint;
    private readonly string _accessToken;

    #endregion

    #region constractor
    public CryptocurrencyRepository(HttpClient httpClient, IOptions<CryptoCurrencyApiSettings> options)
    {

        _baseUrl = options.Value.ExchangeratesapiBaseUrl;
        var url = $"{_baseUrl}";

        _httpClient = httpClient;

        _httpClient.Timeout = TimeSpan.FromSeconds(15);
        _httpClient.BaseAddress = new Uri(url);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _endpoint = options.Value.ExchangeratesapiEndpoint;
        _accessToken = options.Value.ExchangeratesapiAccessToken;
    }

    #endregion
    #region getLastCryptoStatus Service
    /// <summary>
    /// Get latest CryptoStatus from Api
    /// </summary>
    /// <param name="request"></param>
    /// <returns>GetAllCryptoStatusResponseDto</returns>
    public async Task<GetAllCryptoStatusResponseDto> GetLastCryptoStatus(CryptoCurrentRequestDto request)
    {

        #region Check Network Interface
        var checkNetwork = CheckNetWorkInterface();
        if (checkNetwork.success == false)
        {
            return checkNetwork;
        }

        #endregion


        string units = "USD,EUR,BRL,GBP,AUD";
        var path = $"{_endpoint}?access_key={_accessToken}&base={request.CryptoType}&symbols={units}";
        HttpResponseMessage? response = null;
        try
        {

            // ایجاد سیاست Retry
            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = Policy
            .Handle<HttpRequestException>() // هندل کردن HttpRequestException
            .OrResult<HttpResponseMessage>(response =>
                !response.IsSuccessStatusCode) // هندل کردن پاسخ‌های ناموفق
            .WaitAndRetryAsync(6, retryAttempt =>
                TimeSpan.FromSeconds(10), // تأخیر نمایی
                onRetry: (outcome, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry For Get Info ---> {retryCount}: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                    Log.Error("Error :" + $"Retry For Get Info ---> {retryCount}: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                });


            response = await retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(path));

            if (response.IsSuccessStatusCode)
            {
                //Handel Suitable Data
                string jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.
                                            Deserialize<GetAllCryptoStatusResponseDto>(jsonResult)
                                                                        ?? new GetAllCryptoStatusResponseDto();

                result.rates.Remove(request.CryptoType);
                return result;
            }
            else
            {
                //Handel Error Data
                string jsonResult = await response.Content.ReadAsStringAsync();
                var errorResult = JsonSerializer.
                                          Deserialize<ApiErrorResponse>(jsonResult)
                                                                      ?? new ApiErrorResponse();
                var result = new GetAllCryptoStatusResponseDto();
                result.date = DateTime.Now.Date;
                result.error = errorResult.error;
                result.success = false;
                result.@base = "";
                result.timestamp = 0;

                //log in logger
                // ذخیره اطلاعات در لاگ‌ها
                Log.Error("Error :" + errorResult.error);
                return result;




            }
        }
        catch (System.Net.Http.HttpRequestException exRequest)
        {
            //log in logger
            // ذخیره اطلاعات در لاگ‌ها
            Log.Error("Error :" + exRequest.Message);
            //throw Exception
            throw new Exception("ERROR Request...");
        }
        catch (Exception ex)
        {
            //log in logger
            // ذخیره اطلاعات در لاگ‌ها
            Log.Error("Error :" + ex.Message);
            //throw Exception
            throw new Exception("ERROR INTERNAL SERVER...");
        }

    }
    #endregion

    private GetAllCryptoStatusResponseDto CheckNetWorkInterface()
    {
        var result = new GetAllCryptoStatusResponseDto();
        result.success = true;
        result.date = DateTime.Now.Date;
        result.@base = "";
        result.timestamp = 0;
        if (!PingNetwork.IsInternetAvailable())
        {
            result.error = new ApiError("-1", "No Internet Access");
            result.success = false;

            //log in logger
            // ذخیره اطلاعات در لاگ‌ها
            Log.Error("Error :" + "No Internet Access");
        }
        if (!PingNetwork.IsExchangeratesapiAvailable())
        {
            result.error = new ApiError("-1", "Exchangeratesapi Not Available");
            result.success = false;

            //log in logger
            // ذخیره اطلاعات در لاگ‌ها
            Log.Error("Error :" + "Exchangeratesapi Not Available");
        }
        return result;
    }
}
