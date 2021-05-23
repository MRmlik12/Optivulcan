using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
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

        private void AppendToTimetableList(List<string> lessons, int lessonNumber, DateTime startAt, DateTime endAt, List<Teacher> teachers, List<Classroom> classrooms)
        {
            _timetable.TimetableItems.Add(new TimetableItem
            {
                Lesson = lessons,
                LessonNumber = lessonNumber,
                StartAt = startAt,
                EndAt = endAt,
                Teacher = teachers,
                Classroom = classrooms
            });
        }

        private void ScrapTimetable()
        {
            var timetable = (IHtmlTableElement)_document.GetElementsByClassName("tabela")[0];
            foreach (var row in timetable.Rows)
            {
                if (row.Index.Equals(0))
                    continue;
                var lessonNumber = Convert.ToInt32(row.GetElementsByClassName("nr")[0].TextContent);
                var hours = row.GetElementsByClassName("g")[0].TextContent.Split("-");

                foreach (var l in row.GetElementsByClassName("l"))
                {
                    var teachers = new List<Teacher>();
                    var lessons = new List<string>();
                    var classrooms = new List<Classroom>();

                    foreach (var teacher in l.GetElementsByClassName("n"))
                    {
                        teachers.Add(new Teacher
                        {
                            Initials = teacher.TextContent,
                            Href = teacher.GetAttribute("href")
                        });
                    }

                    foreach (var classroom in l.GetElementsByClassName("s"))
                    {
                        classrooms.Add(new Classroom
                        {
                            ClassroomNumber = classroom.TextContent,
                            ClassroomHref = classroom.GetAttribute("href")
                        });
                    }

                    foreach (var subject in l.GetElementsByClassName("p"))
                    {
                        lessons.Add(subject.TextContent);
                    }
                    
                    AppendToTimetableList(lessons, lessonNumber, DateTime.Parse(hours[0]), DateTime.Parse(hours[1]), teachers, classrooms);
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