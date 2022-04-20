using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Optivulcan.Enums;
using Optivulcan.Pocos;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace Optivulcan.Test;

public class TimetableTest : IDisposable
{
    private const int ExpectedListSize = 13;
    private const string ExpectedValidFrom = "10 maja 2021";
    private readonly DateTime _expectedGeneratedDate = DateTime.ParseExact("14.01.2022", "dd.mm.yyyy", CultureInfo.InvariantCulture);

    private readonly WireMockServer _server;

    public TimetableTest()
    {
        _server = WireMockServer.Start();
    }

    public void Dispose()
    {
        _server?.Dispose();
    }

    [Fact]
    public async void TestTimetableAssertLenghtOfReturnedItems()
    {
        _server.Given(Request.Create().WithPath("/plany/o1.html"))
            .RespondWith(Response.Create().WithBody(await File.ReadAllTextAsync("./html/plany/o1.html")));

        var result = await OptivulcanApi.GetTimetable(_server.Urls[0] + "/plany/o1.html");
        Assert.Equal(ExpectedListSize, result.TimetableItems?.Count);
    }

    [Fact]
    public async void CheckSecondElementOfTimetable()
    {
        _server.Given(Request.Create().WithPath("/plany/o1.html"))
            .RespondWith(Response.Create().WithBody(await File.ReadAllTextAsync("./html/plany/o1.html")));

        var result = await OptivulcanApi.GetTimetable(_server.Urls[0] + "/plany/o1.html");
        var expectedItem = new TimetableItem
        {
            Subject = new List<string>
            {
                "religia"
            },
            DayOfWeek = Week.Tuesday,
            LessonNumber = 1,
            StartAt = DateTime.Parse("8:00"),
            EndAt = DateTime.Parse("8:45"),
            Teacher = new List<Teacher>
            {
                new()
                {
                    Initials = "PS",
                    Href = "n1.html"
                }
            },
            Classroom = new List<Classroom>
            {
                new()
                {
                    ClassroomNumber = "2",
                    Href = "s34.html"
                }
            }
        };

        Assert.Equal(ExpectedValidFrom, result.ValidFrom);
        Assert.Equal(_expectedGeneratedDate, result.GeneratedAt);
        Assert.Equal(expectedItem.Subject, result.TimetableItems?[1].Subject);
        Assert.Equal(expectedItem.DayOfWeek, result.TimetableItems?[1].DayOfWeek);
        Assert.Equal(expectedItem.StartAt, result.TimetableItems?[1].StartAt);
        Assert.Equal(expectedItem.EndAt, result.TimetableItems?[1].EndAt);
        Assert.Equal(expectedItem.Teacher[0].ToString(), result.TimetableItems?[1].Teacher?[0].ToString());
        Assert.Equal(expectedItem.Teacher[0].Href, result.TimetableItems?[1].Teacher?[0].Href);
        Assert.Equal(expectedItem.Classroom[0].ToString(), result.TimetableItems?[1].Classroom?[0].ToString());
        Assert.Equal(expectedItem.Classroom[0].Href, result.TimetableItems?[1].Classroom?[0].Href);
    }
}