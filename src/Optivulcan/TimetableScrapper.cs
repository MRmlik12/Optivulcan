using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Optivulcan.Enums;
using Optivulcan.Interfaces;
using Optivulcan.Pocos;

namespace Optivulcan
{
    internal class TimetableScrapper : IScrapper
    {
        private IDocument _document;
        private readonly string _url;
        private readonly Timetable _timetable;
        
        public TimetableScrapper(string branchHref, string url)
        {
            _url = url + branchHref;
            _timetable = new Timetable();
        }
        
        private async Task Initialize()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            _document = await context.OpenAsync(_url);
        }

        private void AppendToTimetableList(List<string> subject, Week dayOfWeek, int lessonNumber, DateTime startAt, DateTime endAt, List<Teacher> teachers, List<Classroom> classrooms)
        {
            _timetable.TimetableItems.Add(new TimetableItem
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
            return l.GetElementsByClassName("s").Select(classroom => new Classroom {ClassroomNumber = classroom.TextContent, ClassroomHref = classroom.GetAttribute("href")}).ToList();
        }

        private static List<Teacher> GetTeachers(IElement l)
        {
            return l.GetElementsByClassName("n").Select(teacher => new Teacher {Initials = teacher.TextContent, Href = teacher.GetAttribute("href")}).ToList();
        }

        private static bool IsLessonEmpty(string content)
        {
            return content == "&nbsp;" || string.IsNullOrEmpty(content);
        }

        private void ScrapTimetable()
        {
            var timetable = (IHtmlTableElement)_document.GetElementsByClassName("tabela")[0];
            foreach (var row in timetable.Rows)
            {
                if (row.Index.Equals(0))
                    continue;
                var dayOfWeek = Week.Monday;
                var lessonNumber = Convert.ToInt32(row.GetElementsByClassName("nr")[0].TextContent);
                var hours = row.GetElementsByClassName("g")[0].TextContent.Split("-");

                foreach (var l in row.GetElementsByClassName("l"))
                {
                    if (!IsLessonEmpty(l.TextContent))
                    {
                        var teachers = GetTeachers(l);
                        var lessons = GetSubjects(l);
                        var classrooms = GetClassrooms(l);

                        AppendToTimetableList(lessons, dayOfWeek, lessonNumber, DateTime.Parse(hours[0]), DateTime.Parse(hours[1]),
                            teachers, classrooms);
                        dayOfWeek++;
                        continue;
                    }

                    dayOfWeek++;
                }
            }
        }

        public async Task<Timetable> GetTimetable()
        {
            await Initialize();
            ScrapTimetable();
            
            return _timetable;
        }
    }
}