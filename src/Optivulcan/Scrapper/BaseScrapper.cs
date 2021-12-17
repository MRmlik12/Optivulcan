using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Optivulcan.Configurations;
using Optivulcan.Interfaces;

namespace Optivulcan.Scrapper;

internal abstract class BaseScrapper : IScrapper
{
    private readonly string? _userAgent;
    protected readonly string Url;
    protected IDocument? Document;

    protected BaseScrapper(string url, string? userAgent)
    {
        _userAgent = userAgent;
        Url = url;
    }

    protected async Task Initialize(string address)
    {
        var context = BrowsingContext.New(AngleSharpConfiguration.GetAngleSharpDefaultConfiguration(_userAgent));

        Document = await context.OpenAsync(address);
    }
}