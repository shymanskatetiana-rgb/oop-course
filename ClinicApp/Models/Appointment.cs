using System;
using ClinicApp.Enums;
using ClinicApp.Utils;
using ClinicApp.Interfaces;

namespace ClinicApp.Models;

public class Appointment : IPayable, ICancellable
{
    private static int _nextId = 1;
    private DateTime _scheduledAt;
    private int _durationMinutes;
    private bool _isPaid;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public AppointmentStatus Status { get; private set; }
    public string Notes { get; private set; }

    public DateTime ScheduledAt
    {
        get => _scheduledAt;
        set => _scheduledAt = value;
    }

    public int DurationMinutes
    {
        get => _durationMinutes;
        set
        {
            ClinicValidator.ValidatePositive(value, nameof(DurationMinutes));
            _durationMinutes = value;
        }
    }

    public DateTime EndsAt
    {
        get { return ScheduledAt.AddMinutes(DurationMinutes); }
    }

    public bool IsUpcoming
    {
        get { return ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled; }
    }

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = AppointmentStatus.Scheduled;
        Notes = "";
        Id = _nextId++;
    }

    public bool IsCancelled
    {
        get { return Status == AppointmentStatus.Cancelled; }
    }

    public string CancellationReason
    {
        get
        {
            if (IsCancelled)
                return Notes;
            return "";
        }
    }

    // Обрано Варіант А (множник знижки в базовому класі).
    // Це дозволяє застосовувати знижку до БУДЬ-ЯКОГО типу прийому (і звичайного, і термінового, і консультації),
    // не створюючи купу нових підкласів (VipUrgent, VipSpecialist), що порушувало б принцип Open/Closed.
    public decimal DiscountMultiplier { get; set; } = 1m;

    public virtual decimal GetCost()
    {
        return (DurationMinutes * 10m) * DiscountMultiplier;
    }

    public virtual string GetDescription()
    {
        return "Прийом";
    }

    public int GetPriority()
    {
        return 3;
    }

    public bool IsPaid
    {
        get { return _isPaid; }
    }

    public void MarkPaid()
    {
        if (!IsCancelled)
        {
            _isPaid = true;
        }
    }

    public bool Cancel(string reason = "")
    {
        if (Status == AppointmentStatus.Scheduled)
        {
            Status = AppointmentStatus.Cancelled;
            if (!string.IsNullOrEmpty(reason))
            {
                Notes = reason;
            }
            return true;
        }
        return false;
    }

    public bool Complete()
    {
        if (Status == AppointmentStatus.Scheduled)
        {
            Status = AppointmentStatus.Completed;
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        string result = $"[{Id}] {GetDescription()} | Пацієнт #{PatientId} → Лікар #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm} | {Status} | {GetCost():F2} грн";

        if (Notes.Length > 0)
        {
            result += $" | {Notes}";
        }
        return result;
    }
}