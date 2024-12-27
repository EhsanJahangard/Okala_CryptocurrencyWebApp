#region using
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Okala_CryptocurrencyWebApp.Contracts;
using Okala_CryptocurrencyWebApp.Dtos.RequestDto;
using Okala_CryptocurrencyWebApp.Dtos.ResponseDto;
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
    //[OutputCache(Duration = 60)] // کش کردن به مدت 60 ثانیه
    [OutputCache(Duration = 60, VaryByQueryKeys = new[] { "cryptoType" })] // کش بر اساس مقدار "cryptoType"
    public async Task<BaseResponseDto<GetAllCryptoStatusResponseDto>> GetCryptoCurrent([FromQuery] CryptoCurrentRequestDto request)
    {

        if (request == null)
        {
            throw new ArgumentNullException("Value cannot be null.");
        }

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
