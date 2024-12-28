using System.Net;

namespace OKala_CryptocurrencyDomain.Dtos.ResponseDto;

public record BaseResponseDto<T>
{
    public T Content { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = "عملیات با موفقیت انجام شد.";


}
