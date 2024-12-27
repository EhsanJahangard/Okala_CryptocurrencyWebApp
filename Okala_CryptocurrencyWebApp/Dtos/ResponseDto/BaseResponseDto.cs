using System.Net;

namespace Okala_CryptocurrencyWebApp.Dtos.ResponseDto;

public record BaseResponseDto<T>
{
    public T Content { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "عملیات با موفقیت انجام شد.";


}
