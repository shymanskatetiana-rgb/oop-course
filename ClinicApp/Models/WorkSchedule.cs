namespace ClinicApp.Models;

public struct WorkSchedule
{
    public int Start { get; }
    public int End { get; }

    public int HoursPerDay
    {
        get
        {
            return End - Start;
        }
    }

    public string Display
    {
        get
        {
            return $"{Start:D2}:00–{End:D2}:00";
        }
    }

    public bool IsNow
    {
        get
        {
            return Contains(DateTime.Now.Hour);
        }
    }

    public bool Contains(int hour)
    {
        return hour >= Start && hour < End;
    }

    public WorkSchedule(int start, int end)
    {
        if (start < 0 || start > 23 || end < 1 || end > 24)
            throw new ArgumentOutOfRangeException("Години роботи мають бути у межах: start (0..23), end (1..24).");

        if (start >= end)
            throw new ArgumentException("Час початку роботи має бути меншим за час завершення.");

        Start = start;
        End = end;
    }   

    public override string ToString()
    {
        return $"{Display} ({HoursPerDay} год)";
    }
}