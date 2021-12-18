using System.Collections.Generic;
using System.Threading.Tasks;
using Optivulcan.Pocos;
using Optivulcan.Scrapper;

namespace Optivulcan;

public static class OptivulcanApi
{
    public static async Task<List<Branch>> GetBranches(string url, string? userAgent = null)
    {
        return await new BranchScrapper(url, userAgent).GetBranches();
    }

    public static async Task<Timetable> GetTimetable(string fullUrl, string? userAgent = null)
    {
        return await new TimetableScrapper(fullUrl, userAgent).GetTimetable();
    }
}