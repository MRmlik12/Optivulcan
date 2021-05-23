using System;
using System.Collections.Generic;
using System.Text;

namespace Optivulcan.Pocos
{
    public class TimetableItem
    {
        public List<string> Lesson { get; set; }
        public int LessonNumber { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public List<Teacher> Teacher { get; set; }
        public List<Classroom> Classroom { get; set; }
    }
}