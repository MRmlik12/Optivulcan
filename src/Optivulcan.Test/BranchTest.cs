using System.IO;
using Optivulcan.Enums;
using Optivulcan.Pocos;
using WireMock.Exceptions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace Optivulcan.Test
{
    public class BranchTest
    {
        private WireMockServer _server;
        private const int Size = 5;

        public BranchTest()
        {
            _server = WireMockServer.Start();
        }
        
        [Fact]
        public async void TestBranchListReturnsCountOfReturnedValue()
        {
            _server.Given(Request.Create().WithPath("/lista.html"))
                .RespondWith(Response.Create().WithBody(File.ReadAllText("./html/lista.html")));
            
            var result = await Optivulcan.GetBranchListAsync(_server.Urls[0]);
            Assert.Equal(Size, result.Count);
        }
        
        [Fact]
        public async void CheckFirstElementFromResponse()
        {
            _server.Given(Request.Create().WithPath("/lista.html"))
                .RespondWith(Response.Create().WithBody(File.ReadAllText("./html/lista.html")));
            
            var result = await Optivulcan.GetBranchListAsync(_server.Urls[0]);
            var expectedItem = new Branch
            {
                Name = "6A",
                Url = "plany/o1.html",
                Type = BranchType.Class
            };
            
            Assert.Equal(result[0].Name, expectedItem.Name);
            Assert.Equal(result[0].Url, expectedItem.Url);
            Assert.Equal(result[0].Type, expectedItem.Type);
        }
    }
}