# Optivulcan
![Build and deploy](https://github.com/MRmlik12/Optivulcan/actions/workflows/build-deploy.yml/badge.svg)
[![NuGet version (Optivulcan)](https://img.shields.io/nuget/v/Optivulcan.svg?style=flat)](https://www.nuget.org/packages/Optivulcan/)

A simple timetable parser for Optivum UONET+ written in C# 

# Example usage

```csharp
var branches = await OptivulcanApi.GetBranches("URL_OF_YOUR_SCHOOL_TIMETABLE"); // Get branches
var timetable = await OptivulcanApi.GetTimetable(branches[index].FullUrl) // Get timetable
```

# Build from source

```bash
$ dotnet restore src
$ dotnet build src
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