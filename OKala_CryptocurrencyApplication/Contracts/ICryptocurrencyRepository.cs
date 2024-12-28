using OKala_CryptocurrencyDomain.Dtos.RequestDto;
using OKala_CryptocurrencyDomain.Dtos.ResponseDto;

namespace OKala_CryptocurrencyApplication.Contracts;

public interface ICryptocurrencyRepository
{
    public Task<GetAllCryptoStatusResponseDto> GetLastCryptoStatus(CryptoCurrentRequestDto request);
}
