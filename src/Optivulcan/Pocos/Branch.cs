using Optivulcan.Enums;

namespace Optivulcan.Pocos;

public class Branch
{
    public string? Name { get; init; }
    public string? Url { get; init; }
    public string? FullUrl { get; init; }
    public BranchType Type { get; init; }
}