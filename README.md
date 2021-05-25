# Optivulcan
![example workflow](https://github.com/MRmlik12/Optivulcan/actions/workflows/build-deploy.yml/badge.svg)
[![NuGet version (SoftCircuits.Silk)](https://img.shields.io/nuget/v/Optivulcan.svg?style=flat)](https://www.nuget.org/packages/Optivulcan/)

A simple timetable parser for Optivum UONET+ written in C# 

# Example usage

```csharp
var branches = await Optivulcan.GetBranchListAsync("URL_OF_YOUR_SCHOOL_TIMETABLE"); // Get branches
var timetable = await Optivulcan.GetTimetableAsync(branches[index].FullUrl) // Get timetable
```

# Build from source

```bash
$ dotnet restore src
$ dotnet build src/Optivulcan
$ dotnet pack -c Release src/Optivulcan
```

# Testing project
```bash
$ dotnet restore src
$ dotnet test src
```

# For contributors

1. Create feature/fix on your new branch. For example `feature/short-desc` or `fix/short-desc`
2. Use english in source code, naming pr, commits itd.

# Used libraries

* [AngleSharp](https://github.com/AngleSharp/AngleSharp)
* [xUnit](https://github.com/xunit/xunit)
* [WireMock](https://github.com/tomakehurst/wiremock)