namespace ClinicApp;

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
            return $"{Start:D2}:00Ц{End:D2}:00";
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
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"{Display} ({HoursPerDay} год)";
    }
}