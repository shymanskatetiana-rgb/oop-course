using System;
using System.IO;
using System.Text;
using ClinicApp.Events;

namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    public void LogInfo(string message)
    {
        Write("INFO", message);
    }

    public void LogWarning(string message)
    {
        Write("WARN", message);
    }

    public void LogError(string message)
    {
        Write("ERROR", message);
    }

    public string[] GetLastLines(int n)
    {
        if (!File.Exists(_logPath) || n <= 0)
        {
            return Array.Empty<string>();
        }

        string[] all = File.ReadAllLines(_logPath, Encoding.UTF8);
        if (n >= all.Length)
        {
            return all;
        }

        string[] result = new string[n];
        int start = all.Length - n;
        for (int i = 0; i < n; i++)
        {
            result[i] = all[start + i];
        }
        return result;
    }

    public void Clear()
    {
        using StreamWriter writer = new StreamWriter(_logPath, false, Encoding.UTF8);
    }

    public bool Exists()
    {
        return File.Exists(_logPath);
    }

    private void Write(string level, string message)
    {
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level,-5}] {message}" + Environment.NewLine;
        File.AppendAllText(_logPath, line, Encoding.UTF8);
    }

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        LogInfo($"Запис #{e.AppointmentId} створено: пацієнт #{e.PatientId} → лікар #{e.DoctorId}");
    }

    public void OnUrgentBooked(object? sender, AppointmentEventArgs e)
    {
        LogWarning($"ТЕРМІНОВИЙ запис #{e.AppointmentId}: {e.Notes}");

        Directory.CreateDirectory("alerts");
        string path = $"alerts/urgent_{DateTime.Now:yyyy-MM-dd}.txt";
        string line = $"[{DateTime.Now:HH:mm:ss}] Запис #{e.AppointmentId}: пацієнт #{e.PatientId} → лікар #{e.DoctorId} | {e.Notes}" + Environment.NewLine;
        File.AppendAllText(path, line, Encoding.UTF8);
    }

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
    {
        string reason = e.Notes.Length > 0 ? $" Причина: {e.Notes}" : "";
        LogInfo($"Запис #{e.AppointmentId} скасовано.{reason}");
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
    {
        LogInfo($"Запис #{e.AppointmentId} завершено.");
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e)
    {
        LogInfo($"Новий пацієнт #{e.PatientId}: {e.FullName}");
    }

    public void OnPaymentReceived(object? sender, PaymentEventArgs e)
    {
        LogInfo($"Оплата запису #{e.AppointmentId}: {e.Amount:F2} грн");
    }

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
    {
        LogInfo($"План #{e.PlanId} завершено: пацієнт #{e.PatientId}, {e.Diagnosis}");
    }
}