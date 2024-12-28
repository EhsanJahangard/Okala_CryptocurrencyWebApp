namespace OKala_CryptocurrencyDomain.Models;

public class ResponseFromApi
{
    public bool success { get; set; }
    public double timestamp { get; set; }
    public string @base { get; set; }
    public DateTime date { get; set; }

    public Dictionary<string, decimal> rates { get; set; }
    public ApiErrorResponse? error { get; set; }

}
