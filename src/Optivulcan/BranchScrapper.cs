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
        private IDocument? _document;
        private readonly string _url;
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
            await Initialize();
            ScrapBranch();

            return _branches;
        }
    }
}