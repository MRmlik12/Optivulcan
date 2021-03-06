using System;
using System.IO;
using Optivulcan.Enums;
using Optivulcan.Pocos;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace Optivulcan.Test;

public class BranchTest : IDisposable
{
    private const int Size = 5;
    private readonly WireMockServer _server;

    public BranchTest()
    {
        _server = WireMockServer.Start();
    }

    public void Dispose()
    {
        _server?.Dispose();
    }

    [Fact]
    public async void TestBranchListReturnsCountOfReturnedValue()
    {
        _server.Given(Request.Create().WithPath("/lista.html"))
            .RespondWith(Response.Create().WithBody(File.ReadAllText("./html/lista.html")));

        var result = await OptivulcanApi.GetBranches(_server.Urls[0]);
        Assert.Equal(Size, result.Count);
    }

    [Fact]
    public async void CheckFirstElementFromResponse()
    {
        _server.Given(Request.Create().WithPath("/lista.html"))
            .RespondWith(Response.Create().WithBody(await File.ReadAllTextAsync("./html/lista.html")));

        var result = await OptivulcanApi.GetBranches(_server.Urls[0]);
        var expectedItem = new Branch
        {
            Name = "6A",
            Url = "/plany/o1.html",
            FullUrl = $"{_server.Urls[0]}plany/o1.html",
            Type = BranchType.Class
        };

        Assert.Equal(expectedItem.Name, result[0].Name);
        Assert.Equal(expectedItem.Url, result[0].Url);
        Assert.Equal(expectedItem.FullUrl, expectedItem.FullUrl);
        Assert.Equal(expectedItem.Type, result[0].Type);
    }
}