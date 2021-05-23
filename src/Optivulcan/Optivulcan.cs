using System.Collections.Generic;
using System.Threading.Tasks;
using Optivulcan.Pocos;

namespace Optivulcan
{
    public static class Optivulcan
    {
        public static async Task<List<Branch>> GetBranchListAsync(string url)
        {
            return await new BranchScrapper().GetBranches(url);
        }

        public static async Task<Timetable> GetTimetableAsync(string url, string branchHref)
        {
            return await new TimetableScrapper(branchHref, url).GetTimetable();
        }
    }
}