using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class PatientPassportWriter
{
    private readonly Clinic _clinic;
    private readonly string _directory;

    public PatientPassportWriter(Clinic clinic, string directory = "patients")
    {
        _clinic = clinic;
        _directory = directory;
        Directory.CreateDirectory(_directory);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e)
    {
        WritePassport(e.PatientId);
    }

    private void WritePassport(int patientId)
    {
        Patient? patient = _clinic.Patients.FindById(patientId);
        if (patient == null)
        {
            return;
        }

        string path = Path.Combine(_directory, $"passport_{patientId}.txt");

        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);

        writer.WriteLine($"=== Паспорт пацієнта #{patient.Id} ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine($"Ім'я: {patient.FullName}");
        writer.WriteLine($"Дата народження: {patient.DateOfBirth:dd.MM.yyyy} ({ClinicFormatter.FormatAge(patient.Age)})");
        writer.WriteLine($"Група крові: {ClinicFormatter.FormatBloodType(patient.BloodType)} Телефон: {ClinicFormatter.FormatPhone(patient.Phone)}");

        MedicalRecord[] records = _clinic.MedicalRecords.GetByPatient(patientId);

        WriteDiagnoses(writer, records);
        WriteLabResults(writer, records);
        WritePrescriptions(writer, records);
        WriteAppointments(writer, patientId);
        WriteTreatmentPlans(writer, patientId);

        writer.WriteLine($"Заборгованість: {_clinic.Billing.GetPatientDebt(patientId):F2} грн");
    }

    private void WriteDiagnoses(StreamWriter writer, MedicalRecord[] records)
    {
        bool printed = false;

        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Diagnosis d)
            {
                if (!printed)
                {
                    writer.WriteLine("-- Діагнози --");
                    printed = true;
                }

                string line = $"{d.DiagnosisCode}: {d.Description}";
                if (d.IsChronic)
                {
                    line += " [хронічне]";
                }
                writer.WriteLine(line);
            }
        }
    }

    private void WriteLabResults(StreamWriter writer, MedicalRecord[] records)
    {
        bool printed = false;

        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is LabResult l)
            {
                if (!printed)
                {
                    writer.WriteLine("-- Аналізи --");
                    printed = true;
                }

                string value = l.Value.ToString(CultureInfo.InvariantCulture);
                string line = $"{l.TestName}: {value} {l.Unit} (норма: {l.ReferenceRange})";
                if (!l.IsNormal)
                {
                    line += " ⚠ поза нормою";
                }
                writer.WriteLine(line);
            }
        }
    }

    private void WritePrescriptions(StreamWriter writer, MedicalRecord[] records)
    {
        bool printed = false;

        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Prescription p)
            {
                if (!printed)
                {
                    writer.WriteLine("-- Рецепти --");
                    printed = true;
                }

                string line = $"{p.MedicationName} {p.Dosage} × {p.DurationDays} днів";
                if (!string.IsNullOrEmpty(p.Instructions))
                {
                    line += $" ({p.Instructions})";
                }
                writer.WriteLine(line);
            }
        }
    }

    private void WriteAppointments(StreamWriter writer, int patientId)
    {
        Appointment[] apps = _clinic.Appointments.GetByPatient(patientId);
        if (apps.Length == 0)
        {
            return;
        }

        writer.WriteLine("-- Записи --");
        for (int i = 0; i < apps.Length; i++)
        {
            Appointment a = apps[i];
            writer.WriteLine($"[{a.Id}] {a.GetDescription()} | {a.ScheduledAt:dd.MM.yyyy HH:mm} | {a.Status} | {a.GetCost():F2} грн");
        }
    }

    private void WriteTreatmentPlans(StreamWriter writer, int patientId)
    {
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetByPatient(patientId);
        if (plans.Length == 0)
        {
            return;
        }

        writer.WriteLine("-- Плани лікування --");
        for (int i = 0; i < plans.Length; i++)
        {
            TreatmentPlan plan = plans[i];
            writer.WriteLine($"[{plan.Id}] {plan.Diagnosis} | {plan.DurationDays} днів | {plan.Status}");
        }
    }
}