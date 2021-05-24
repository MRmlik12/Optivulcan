using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Optivulcan.Enums;
using Optivulcan.Interfaces;
using Optivulcan.Pocos;

namespace Optivulcan
{
    internal class BranchScrapper : IScrapper
    {
        private IDocument _document;
        private string _url;
        private readonly List<Branch> _branches;

        public BranchScrapper(string url)
        {
            _branches = new List<Branch>();
            _url = url;
        }
        
        private async Task Initialize()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            _document = await context.OpenAsync($"{_url}/lista.html");
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
            var url = item.GetAttribute("href");
            _branches.Add(new Branch
            {
                Name = item.TextContent,
                Url = $"/{url}",
                FullUrl = $"{_url}/{url}",
                Type = GetBranchType(url.Split('/')[1].ToCharArray()[0])
            });
        }

        private void ScrapBranch()
        {
            var items = _document.QuerySelectorAll<IHtmlAnchorElement>("ul li a");
            foreach (var item in items) 
            { 
                if (!item.GetAttribute("target").Equals("_blank"))
                    AppendBranchItem(item);
            }
        }

        public async Task<List<Branch>> GetBranches()
        {
            await Initialize();
            ScrapBranch();

            return _branches;
        }
    }
}