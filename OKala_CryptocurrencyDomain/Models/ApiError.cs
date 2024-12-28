namespace OKala_CryptocurrencyDomain.Models;


public class ApiError
{
    public ApiError(string? code, string? info)
    {
        this.code = code;
        this.info = info;
    }
    public ApiError()
    {

    }
    public string? code { get; set; }
    public string? info { get; set; }
}
