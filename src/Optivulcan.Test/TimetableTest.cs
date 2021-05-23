using System.IO;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace Optivulcan.Test
{
    public class TimetableTest
    {
        private const int ExpectedListSize = 13;
        private WireMockServer _server;

        public TimetableTest()
        {
            _server = WireMockServer.Start();
        }
        
        [Fact]
        public async void TestTimetableAssertLenghtOfReturnedItems()
        {
            _server.Given(Request.Create().WithPath("/plany/o1.html"))
                .RespondWith(Response.Create().WithBody(await File.ReadAllTextAsync("./html/plany/o1.html")));
            
            var result = await Optivulcan.GetTimetableAsync(_server.Urls[0], "/plany/o1.html");
            Assert.Equal(ExpectedListSize, result.TimetableItems.Count);
        }
    }
}