using System.Net.Http;
using AngleSharp;
using AngleSharp.Io.Network;

namespace Optivulcan.Configurations;

internal static class AngleSharpConfiguration
{
    public static IConfiguration GetAngleSharpDefaultConfiguration(string? userAgent)
    {
        var httpClient = new HttpClient(
            new HttpClientHandler
            {
                UseCookies = false,
                AllowAutoRedirect = false
            }
        );
        httpClient.DefaultRequestHeaders.Add("User-Agent",
            userAgent ?? $"Optivulcan {typeof(Api).Assembly.GetName().Version}");

        var config = Configuration.Default
            .With(new HttpClientRequester(httpClient)).WithDefaultLoader();

        return config;
    }
}