using System;
using System.IO;
using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionEventTracker
{
    private readonly Clinic _clinic;

    public int PatientsAdded { get; private set; }
    public int AppointmentsBooked { get; private set; }
    public int UrgentBooked { get; private set; }
    public int AppointmentsCancelled { get; private set; }
    public int AppointmentsCompleted { get; private set; }
    public int PaymentsReceived { get; private set; }
    public int PlansCompleted { get; private set; }

    public SessionEventTracker(Clinic clinic)
    {
        _clinic = clinic;
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e) => PatientsAdded++;

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e) => AppointmentsBooked++;

    public void OnUrgentBooked(object? sender, AppointmentEventArgs e) => UrgentBooked++;

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
    {
        AppointmentsCancelled++;

        if (_clinic.WaitingRoom.Count > 0)
        {
            Patient next = _clinic.WaitingRoom.Peek();
            Console.WriteLine($"[ЧЕРГА] Слот звільнився. Наступний: {next.FullName}");
        }
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) => AppointmentsCompleted++;

    public void OnPaymentReceived(object? sender, PaymentEventArgs e) => PaymentsReceived++;

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) => PlansCompleted++;

    public void PrintSummary()
    {
        string[] lines = BuildSummaryLines();
        for (int i = 0; i < lines.Length; i++)
        {
            Console.WriteLine(lines[i]);
        }
    }

    public void SaveSummary(string path = "session_summary.txt")
    {
        string[] lines = BuildSummaryLines();

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        for (int i = 0; i < lines.Length; i++)
        {
            writer.WriteLine(lines[i]);
        }
    }

    private string[] BuildSummaryLines()
    {
        return new string[]
        {
            "=== Підсумок сесії ===",
            $"Пацієнтів додано: {PatientsAdded}",
            $"Записів створено: {AppointmentsBooked} (термінових: {UrgentBooked})",
            $"Скасовано: {AppointmentsCancelled}",
            $"Завершено: {AppointmentsCompleted}",
            $"Оплат: {PaymentsReceived}",
            $"Планів завершено: {PlansCompleted}"
        };
    }
}