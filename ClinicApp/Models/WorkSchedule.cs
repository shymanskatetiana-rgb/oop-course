using System;

namespace ClinicApp.Models;

public struct WorkSchedule
{
    public int Start { get; }
    public int End { get; }

    public int HoursPerDay
    {
        get { return End - Start; }
    }

    public string Display
    {
        get { return $"{Start:D2}:00–{End:D2}:00"; }
    }

    public bool IsNow
    {
        get { return Contains(DateTime.Now.Hour); }
    }

    public bool Contains(int hour)
    {
        return hour >= Start && hour < End;
    }

    public WorkSchedule(int start, int end)
    {
        if (start < 0 || start > 23)
            throw new ArgumentOutOfRangeException(nameof(start), "Година початку роботи має бути в межах 0..23.");

        if (end < 1 || end > 24)
            throw new ArgumentOutOfRangeException(nameof(end), "Година завершення роботи має бути в межах 1..24.");

        if (start >= end)
            throw new ArgumentException("Час початку роботи має бути меншим за час завершення.", nameof(start));

        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"{Display} ({HoursPerDay} год)";
    }
}