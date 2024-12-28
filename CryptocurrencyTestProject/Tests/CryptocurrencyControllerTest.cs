using Moq;
using OKala_CryptocurrencyApplication.Contracts;
using Okala_CryptocurrencyWebApp.Controllers;
using OKala_CryptocurrencyDomain.Dtos.RequestDto;
using OKala_CryptocurrencyDomain.Dtos.ResponseDto;

namespace CryptocurrencyTestProject.Tests;

public class CryptocurrencyControllerTests
{
    private readonly Mock<ICryptocurrencyRepository> _cryptoServiceMock;
    private readonly CryptocurrencyController _controller;

    public CryptocurrencyControllerTests()
    {
        // Mock سرویس Repository
        _cryptoServiceMock = new Mock<ICryptocurrencyRepository>();

        // ایجاد نمونه کنترلر
        _controller = new CryptocurrencyController(_cryptoServiceMock.Object);
    }

    [Fact]
    public async Task GetCryptoCurrent_ShouldReturn200WithData_WhenServiceReturnsResult()
    {
        // Arrange: شبیه‌سازی داده ورودی و خروجی
        var requestDto = new CryptoCurrentRequestDto
        {
            CryptoType = "EUR"
        };

        var responseDto = new GetAllCryptoStatusResponseDto
        {
            success = true
        };

        _cryptoServiceMock
            .Setup(x => x.GetLastCryptoStatus(requestDto))
            .ReturnsAsync(responseDto);

        // Act: فراخوانی متد
        var result = await _controller.GetCryptoCurrent(requestDto);

        // Assert: بررسی خروجی
        var okResult = Assert.IsType<BaseResponseDto<GetAllCryptoStatusResponseDto>>(result);
        Assert.NotNull(okResult.Content);
        Assert.Contains("USD", okResult.Content.rates);
        Assert.Contains("GBP", okResult.Content.rates);

    }

    [Fact]
    public async Task GetCryptoCurrent_ShouldThrowException_WhenServiceThrowsException()
    {
        // Arrange: شبیه‌سازی رفتار خطا
        var requestDto = new CryptoCurrentRequestDto
        {
            CryptoType = "USD"

        };

        _cryptoServiceMock
            .Setup(x => x.GetLastCryptoStatus(requestDto))
            .ThrowsAsync(new Exception("Service error"));

        // Act & Assert: بررسی Exception
        var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetCryptoCurrent(requestDto));
        Assert.Equal("ERROR INTERNAL SERVER...", exception.Message);
    }

    [Fact]
    public async Task GetCryptoCurrent_ShouldReturn400_WhenRequestIsInvalid()
    {
        // Arrange: درخواست نامعتبر
        CryptoCurrentRequestDto requestDto = null;

        // Act
        var result = await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.GetCryptoCurrent(requestDto));

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Value cannot be null.", result.Message);
    }
}