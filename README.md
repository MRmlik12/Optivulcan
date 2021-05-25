# Optivulcan

A simple timetable parser for Optivum UONET+ written in C# 

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