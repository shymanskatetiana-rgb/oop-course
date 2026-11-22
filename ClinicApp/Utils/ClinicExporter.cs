using System;
using System.IO;
using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class ClinicExporter
{
    private readonly Clinic _clinic;
    private readonly string _baseDir;

    public ClinicExporter(Clinic clinic, string baseDir = "reports")
    {
        _clinic = clinic;
        _baseDir = baseDir;
    }

    public string ExportPatients()
    {
        Patient[] patients = _clinic.Patients.GetAll();
        string[] lines = new string[patients.Length];

        for (int i = 0; i < patients.Length; i++)
        {
            Patient p = patients[i];
            lines[i] = $"[{p.Id}] {p.FullName} | {ClinicFormatter.FormatAge(p.Age)} | " +
                       $"{ClinicFormatter.FormatBloodType(p.BloodType)} | {ClinicFormatter.FormatPhone(p.Phone)}";
        }

        return WriteReport("patients.txt", "Пацієнти", lines, $"Всього: {patients.Length}");
    }

    public string ExportAppointments()
    {
        Appointment[] apps = _clinic.Appointments.GetAll();
        string[] lines = new string[apps.Length];

        for (int i = 0; i < apps.Length; i++)
        {
            Appointment a = apps[i];
            Patient? p = _clinic.Patients.FindById(a.PatientId);
            Doctor? d = _clinic.Doctors.FindById(a.DoctorId);

            string pName = p != null ? p.FullName : $"#{a.PatientId}";
            string dName = d != null ? d.FullName : $"#{a.DoctorId}";

            lines[i] = $"[{a.Id}] {a.GetDescription()} | {pName} → {dName} | " +
                       $"{a.ScheduledAt:dd.MM.yyyy HH:mm} | {a.Status} | {a.GetCost():F2} грн";
        }

        return WriteReport("appointments.txt", "Записи", lines, $"Всього: {apps.Length}");
    }

    public string ExportBilling()
    {
        IPayable[] unpaid = _clinic.Billing.GetAllUnpaid();
        string[] lines = new string[unpaid.Length];

        for (int i = 0; i < unpaid.Length; i++)
        {
            if (unpaid[i] is Appointment a)
            {
                lines[i] = $"[{a.Id}] пацієнт #{a.PatientId} | {a.ScheduledAt:dd.MM.yyyy} | {unpaid[i].GetCost():F2} грн";
            }
            else
            {
                lines[i] = $"Платіж | {unpaid[i].GetCost():F2} грн";
            }
        }

        string summary = $"Неоплачених: {unpaid.Length} | Загальний борг: {_clinic.Billing.GetTotalDebt():F2} грн";
        return WriteReport("billing.txt", "Борги", lines, summary);
    }

    public string ExportTreatmentPlans()
    {
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetAll();
        string[] lines = new string[plans.Length];

        for (int i = 0; i < plans.Length; i++)
        {
            TreatmentPlan plan = plans[i];
            lines[i] = $"#{plan.Id} | пацієнт #{plan.PatientId} | {plan.Diagnosis} | " +
                       $"{plan.DurationDays} дн. | {plan.Status}";
        }

        return WriteReport("treatment_plans.txt", "Плани лікування", lines, $"Всього: {plans.Length}");
    }

    public string[] ExportAll()
    {
        string[] paths = new string[4];
        paths[0] = ExportPatients();
        paths[1] = ExportAppointments();
        paths[2] = ExportBilling();
        paths[3] = ExportTreatmentPlans();
        return paths;
    }

    private string EnsureDirectory()
    {
        string dir = Path.Combine(_baseDir, DateTime.Today.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    private string WriteReport(string fileName, string title, string[] lines, string summary)
    {
        string path = Path.Combine(EnsureDirectory(), fileName);

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine($"=== {title} — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));

        for (int i = 0; i < lines.Length; i++)
        {
            writer.WriteLine($"{i + 1,3}. {lines[i]}");
        }

        writer.WriteLine(new string('-', 40));
        writer.WriteLine(summary);

        return path;
    }
}