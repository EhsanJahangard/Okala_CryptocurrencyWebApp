#region using
using Microsoft.Extensions.Options;
using Okala_CryptocurrencyWebApp.Configurations;
using Okala_CryptocurrencyWebApp.Contracts;
using Okala_CryptocurrencyWebApp.Dtos.RequestDto;
using Okala_CryptocurrencyWebApp.Dtos.ResponseDto;
using Okala_CryptocurrencyWebApp.Models;
using Serilog;
using System.Net.Http.Headers;
using System.Text.Json; 
#endregion

namespace Okala_CryptocurrencyWebApp.Services;

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
        string units = "USD,EUR,BRL,GBP,AUD";
        var path = $"{_endpoint}?access_key={_accessToken}&base={request.CryptoType}&symbols={units}";
        HttpResponseMessage? response = null;
        try
        {
            response = await _httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                //Handel Suitable Data
                string jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.
                                            Deserialize<GetAllCryptoStatusResponseDto>(jsonResult)
                                                                        ?? new GetAllCryptoStatusResponseDto();
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
}
