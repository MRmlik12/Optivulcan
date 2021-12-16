using System.Collections.Generic;
using System.Threading.Tasks;
using Optivulcan.Pocos;
using Optivulcan.Scrapper;

namespace Optivulcan;

public static class OptivulcanApi
{
    public static async Task<List<Branch>> GetBranchListAsync(string url, string? userAgent = null)
        => await new BranchScrapper(url, userAgent).GetBranches();

    public static async Task<Timetable> GetTimetableAsync(string fullUrl, string? userAgent = null)
        => await new TimetableScrapper(fullUrl, userAgent).GetTimetable();
}