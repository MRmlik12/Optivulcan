using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.XPath;
using Optivulcan.Enums;
using Optivulcan.Pocos;

namespace Optivulcan.Scrapper;

internal class TimetableScrapper : BaseScrapper
{
    private readonly Timetable _timetable = new();

    public TimetableScrapper(string url, string? userAgent) : base(url, userAgent)
    {
    }

    private void AppendToTimetableList(List<string> subject, Week dayOfWeek, int lessonNumber, DateTime startAt,
        DateTime endAt, List<Teacher> teachers, List<Classroom> classrooms)
    {
        _timetable.TimetableItems?.Add(new TimetableItem
        {
            Subject = subject,
            DayOfWeek = dayOfWeek,
            LessonNumber = lessonNumber,
            StartAt = startAt,
            EndAt = endAt,
            Teacher = teachers,
            Classroom = classrooms
        });
    }

    private static List<string> GetSubjects(IElement l)
    {
        return l.GetElementsByClassName("p").Select(subject => subject.TextContent).ToList();
    }

    private static List<Classroom> GetClassrooms(IElement l)
    {
        return l.GetElementsByClassName("s").Select(classroom => new Classroom
        { ClassroomNumber = classroom.TextContent, Href = classroom.GetAttribute("href") }).ToList();
    }

    private DateTime? GetTimetableGeneratedDate()
    {
        var rawDate = Document?.Body.SelectSingleNode("/html/body/div/table/tbody/tr[3]/td[2]/table/tbody/tr/td[1]")
            .TextContent.Trim().Split(" ")[1];
        var matchedDate = Regex.Match(rawDate?.Replace("za\n", "").Replace("\nza", "")!,
            "^([0-3][0-9]|(3)[0-1])(\\.)(((0)[0-9])|((1)[0-2]))(\\.)\\d{4}$");

        if (DateTime.TryParseExact(matchedDate.Value, "dd.mm.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            return parsedDate;

        return null;
    }

    private string? GetTimetableValidFrom()
    {
        var rawDate = Document?.Body.SelectSingleNode("/html/body/div/table/tbody/tr[2]/td").TextContent.Split(":")[1]
            .Replace("r.", "").Replace("roku", "").Trim();

        return rawDate;
    }

    private static List<Teacher> GetTeachers(IElement l)
    {
        return l.GetElementsByClassName("n").Select(teacher => new Teacher
        { Initials = teacher.TextContent, Href = teacher.GetAttribute("href") }).ToList();
    }

    private static bool IsLessonEmpty(string content)
    {
        return string.IsNullOrWhiteSpace(content);
    }

    private void ScrapTimetable()
    {
        if (Document == null) return;
        var timetable = (IHtmlTableElement)Document.GetElementsByClassName("tabela")[0];

        _timetable.GeneratedAt = GetTimetableGeneratedDate();
        _timetable.ValidFrom = GetTimetableValidFrom();

        foreach (var row in timetable.Rows)
        {
            if (row.Index.Equals(0))
                continue;

            var dayOfWeek = Week.Monday;
            var lessonNumber = Convert.ToInt32(row.GetElementsByClassName("nr")[0].TextContent);
            var hours = row.GetElementsByClassName("g")[0].TextContent.Split("-");

            foreach (var l in row.GetElementsByClassName("l"))
            {
                if (IsLessonEmpty(l.TextContent))
                {
                    dayOfWeek++;
                    continue;
                }

                var teachers = GetTeachers(l);
                var lessons = GetSubjects(l);
                var classrooms = GetClassrooms(l);

                AppendToTimetableList(lessons, dayOfWeek, lessonNumber, DateTime.Parse(hours[0]),
                    DateTime.Parse(hours[1]),
                    teachers, classrooms);
                dayOfWeek++;
            }
        }
    }

    public async Task<Timetable> GetTimetable()
    {
        await Initialize(Url);
        ScrapTimetable();

        return _timetable;
    }
}