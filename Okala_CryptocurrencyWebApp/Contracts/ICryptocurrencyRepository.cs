using Okala_CryptocurrencyWebApp.Dtos.RequestDto;
using Okala_CryptocurrencyWebApp.Dtos.ResponseDto;

namespace Okala_CryptocurrencyWebApp.Contracts;

public interface ICryptocurrencyRepository
{
    public Task<GetAllCryptoStatusResponseDto> GetLastCryptoStatus(CryptoCurrentRequestDto request);
}
