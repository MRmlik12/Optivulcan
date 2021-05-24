using System.Collections.Generic;
using System.Threading.Tasks;
using Optivulcan.Pocos;

namespace Optivulcan
{
    public static class Optivulcan
    {
        public static async Task<List<Branch>> GetBranchListAsync(string url)
        {
            return await new BranchScrapper(url).GetBranches();
        }

        public static async Task<Timetable> GetTimetableAsync(string fullUrl)
        {
            return await new TimetableScrapper(fullUrl).GetTimetable();
        }
    }
}