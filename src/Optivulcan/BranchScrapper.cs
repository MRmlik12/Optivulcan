using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Optivulcan.Configurations;
using Optivulcan.Enums;
using Optivulcan.Interfaces;
using Optivulcan.Pocos;

namespace Optivulcan;

internal class BranchScrapper : IScrapper
{
    private readonly List<Branch> _branches;
    private readonly string _url;
    private readonly string? _userAgent;
    private IDocument? _document;

    public BranchScrapper(string url, string? userAgent)
    {
        _userAgent = userAgent;
        _branches = new List<Branch>();
        _url = url;
    }

    private async Task Initialize(string address)
    {
        var context = BrowsingContext.New(AngleSharpConfiguration.GetAngleSharpDefaultConfiguration(_userAgent));
        
        _document = await context.OpenAsync(address);
    }

    private static BranchType GetBranchType(char branch)
    {
        return branch switch
        {
            'o' => BranchType.Class,
            'n' => BranchType.Teacher,
            's' => BranchType.ClassRoom,
            _ => BranchType.Other
        };
    }

    private void AppendBranchItem(IHtmlAnchorElement item)
    {
        _branches.Add(new Branch
        {
            Name = item.TextContent,
            Url = item.PathName,
            FullUrl = $"{item.Href}/{item.PathName}",
            Type = GetBranchType(item.PathName.Split('/')[2].ToCharArray()[0])
        });
    }

    private void ScrapBranch()
    {
        var items = _document?.QuerySelectorAll<IHtmlAnchorElement>("ul li a");
        if (items == null) return;

        foreach (var item in items)
        {
            var target = item.Target;

            if (target!.Equals("_blank")) continue;

            AppendBranchItem(item);
        }
    }

    public async Task<List<Branch>> GetBranches()
    {
        await Initialize($"{_url}/lista.html");
        ScrapBranch();

        return _branches;
    }
}