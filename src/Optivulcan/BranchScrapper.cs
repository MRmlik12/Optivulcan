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
        private readonly List<Branch> _branches;

        public BranchScrapper()
        {
            _branches = new List<Branch>();
        }
        
        private async Task Initialize(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            _document = await context.OpenAsync($"{url}/lista.html");
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
                Url = url,
                Type = GetBranchType(url.Split('/')[1].ToCharArray()[0])
            });
        }

        public async Task<List<Branch>> GetBranches(string url)
        {
            await Initialize(url);
            var items = _document.QuerySelectorAll<IHtmlAnchorElement>("ul li a");
            foreach (var item in items) 
            { 
                if (!item.GetAttribute("target").Equals("_blank"))
                    AppendBranchItem(item);
            }

            return _branches;
        }
    }
}