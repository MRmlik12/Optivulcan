using System.IO;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace Optivulcan.Test
{
    public class TimetableTest
    {
        private WireMockServer _server;

        public TimetableTest()
        {
            _server = WireMockServer.Start();
        }
        
        [Fact]
        public async void TestTimetableAssertLenghtOfReturnedItems()
        {
            _server.Given(Request.Create().WithPath("/lista.html"))
                .RespondWith(Response.Create().WithBody(File.ReadAllText("./html/plany/o1.html")));
            
            var result = await Optivulcan.GetTimetableAsync(_server.Urls[0], "/plany/o1.html");
            Assert.True(true);
        }
    }
}