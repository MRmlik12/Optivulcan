using System;
using System.Collections.Generic;
using Optivulcan.Enums;

namespace Optivulcan.Pocos;

public class TimetableItem
{
    public List<string>? Subject { get; init; }
    public Week DayOfWeek { get; init; }
    public int LessonNumber { get; init; }
    public DateTime StartAt { get; init; }
    public DateTime EndAt { get; init; }
    public List<Teacher>? Teacher { get; init; }
    public List<Classroom>? Classroom { get; init; }
}