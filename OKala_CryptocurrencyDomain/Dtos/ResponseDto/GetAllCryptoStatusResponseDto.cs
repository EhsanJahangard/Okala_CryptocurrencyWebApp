using OKala_CryptocurrencyDomain.Models;

namespace OKala_CryptocurrencyDomain.Dtos.ResponseDto;

public record GetAllCryptoStatusResponseDto
{
   
    public bool success { get; set; }
    public double timestamp { get; set; }
    public string @base { get; set; }
    public DateTime date { get; set; }

    public Dictionary<string, decimal> rates { get; set; }
    public ApiError? error { get; set; }

}
