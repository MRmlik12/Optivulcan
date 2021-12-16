using System.Collections.Generic;
using System.Threading.Tasks;
using Optivulcan.Pocos;

namespace Optivulcan;

public static class Api
{
    public static async Task<List<Branch>> GetBranchListAsync(string url, string? userAgent = null)
    {
        return await new BranchScrapper(url, userAgent).GetBranches();
    }

    public static async Task<Timetable> GetTimetableAsync(string fullUrl, string? userAgent = null)
    {
        return await new TimetableScrapper(fullUrl, userAgent).GetTimetable();
    }
}