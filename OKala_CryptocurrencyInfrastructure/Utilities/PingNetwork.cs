using System.Net.NetworkInformation;

namespace OKala_CryptocurrencyInfrastructure.Utilities;

public static class PingNetwork
{
    public static bool IsInternetAvailable()
    {
        try
        {
            using (Ping ping = new Ping())
            {
                PingReply reply = ping.Send("8.8.8.8", 1000);
                return reply.Status == IPStatus.Success;
            }
        }
        catch
        {
            return false;
        }
    }
    public static bool IsExchangeratesapiAvailable()
    {
        try
        {
            using (Ping ping = new Ping())
            {
                PingReply reply = ping.Send("api.exchangeratesapi.io", 1000);
                return reply.Status == IPStatus.Success;
            }
        }
        catch
        {
            return false;
        }
    }
}
