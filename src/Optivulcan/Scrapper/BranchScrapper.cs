using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Optivulcan.Enums;
using Optivulcan.Pocos;

namespace Optivulcan.Scrapper;

internal class BranchScrapper : BaseScrapper
{
    private readonly List<Branch> _branches = new();

    public BranchScrapper(string url, string? userAgent) : base(url, userAgent)
    {
    }

    private static BranchType GetBranchType(char branch)
    {
        return branch switch
        {
            'o' => BranchType.Class,
            'n' => BranchType.Teacher,
            's' => BranchType.Classroom,
            _ => BranchType.Other
        };
    }

    private void AppendBranchItem(IHtmlAnchorElement item)
    {
        _branches.Add(new Branch
        {
            Name = item.TextContent,
            Url = $"/{item.GetAttribute("href")}",
            FullUrl = $"{item.Href}",
            Type = GetBranchType(item.GetAttribute("href")!.Split('/')[1].ToCharArray()[0])
        });
    }

    private void ScrapBranch()
    {
        var items = Document?.QuerySelectorAll<IHtmlAnchorElement>("ul li a");
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
        await Initialize($"{Url}/lista.html");
        ScrapBranch();

        return _branches;
    }
}