
using FluentValidation;
using Okala_CryptocurrencyWebApp.Dtos.RequestDto;
public class CryptoCurrentRequestDtoValidator : AbstractValidator<CryptoCurrentRequestDto>
{
    public CryptoCurrentRequestDtoValidator()
    {
        RuleFor(x => x.CryptoType)          
            .Matches("^(USD|EUR|BRL|GBP|AUD)$").WithMessage("Invalid Currency");

        RuleFor(x => x.CryptoType)
            .NotEmpty().WithMessage("Value cannot be null.");

    }
}