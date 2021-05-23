using System;
using System.Collections.Generic;

namespace Optivulcan.Pocos
{
    public class Timetable
    {
        public List<TimetableItem> TimetableItems { get; set; }

        public Timetable()
        {
            TimetableItems = new List<TimetableItem>();
        }
    }
}