﻿#region using
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using OKala_CryptocurrencyApplication.Contracts;
using OKala_CryptocurrencyDomain.Dtos.RequestDto;
using OKala_CryptocurrencyDomain.Dtos.ResponseDto;
using Serilog; 
#endregion
namespace Okala_CryptocurrencyWebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptocurrencyController : ControllerBase
{
    #region private_Fields
    private readonly ICryptocurrencyRepository _cryptoService;
    #endregion
    #region constractor
    public CryptocurrencyController(ICryptocurrencyRepository cryptoService)
    {
        _cryptoService = cryptoService;
    }
    #endregion

    #region getLastCryptocurrency_Api

    [HttpGet("GetLastCryptocurrency")]
    [ProducesResponseType(typeof(BaseResponseDto<GetAllCryptoStatusResponseDto>), 200)]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
    [Tags("GetCryptoCurrent - وضعیت اخیر رمز ارزها")]
    [EndpointDescription("USD,EUR,BRL,GBP,AUD وضعیت اخیر رمز ارزها")]
    [OutputCache(Duration = 30, VaryByQueryKeys = new[] { "cryptoType" })] // کش بر اساس مقدار "cryptoType"
    public async Task<BaseResponseDto<GetAllCryptoStatusResponseDto>> GetCryptoCurrent([FromQuery] CryptoCurrentRequestDto request)
    {

        BaseResponseDto<GetAllCryptoStatusResponseDto> response = new BaseResponseDto<GetAllCryptoStatusResponseDto>();
        try
        {

            var result = await _cryptoService.GetLastCryptoStatus(request);
            response.Content = result;
            return response;
        }
        catch (ArgumentNullException ex)
        {

            //log in logger
            // ذخیره اطلاعات در لاگ‌ها
            Log.Error("Error :" + ex.Message);
            //throw Exception
            throw new Exception("Value cannot be null.");
        }
        catch (Exception ex)
        {

            //log in logger
            Log.Error("Error : ERROR INTERNAL SERVER...");
            //throw Exception
            throw new Exception("ERROR INTERNAL SERVER...");
        }
    }


    #endregion
}
