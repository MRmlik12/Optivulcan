using System;
using System.Collections.Generic;

namespace Optivulcan.Pocos;

public class Timetable
{
    public Timetable()
    {
        TimetableItems = new List<TimetableItem>();
    }

    public List<TimetableItem>? TimetableItems { get; set; }
    public DateTime? GeneratedAt { get; set; }
    public string? ValidFrom { get; set; }
}