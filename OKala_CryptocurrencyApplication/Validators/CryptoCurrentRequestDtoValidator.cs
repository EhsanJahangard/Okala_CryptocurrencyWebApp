
using FluentValidation;
using OKala_CryptocurrencyDomain.Dtos.RequestDto;
public class CryptoCurrentRequestDtoValidator : AbstractValidator<CryptoCurrentRequestDto>
{
    public CryptoCurrentRequestDtoValidator()
    {
        RuleFor(x => x.CryptoType)
            .Matches("^(USD|EUR|BRL|GBP|AUD)$").WithMessage("Invalid Currency Type, Please Enter Valid Currency Type Such as USD|EUR|BRL|GBP|AUD ");

        RuleFor(x => x.CryptoType)
            .NotNull().WithMessage("Value cannot be null.");
        RuleFor(x => x.CryptoType)
           .NotEmpty().WithMessage("Value cannot be empty.");

    }
}