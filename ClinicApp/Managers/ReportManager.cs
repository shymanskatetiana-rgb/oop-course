using System.Collections.Generic;
using System.Linq;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class ReportManager
{
    private readonly AppointmentManager _appointments;
    private readonly DoctorManager _doctors;
    private readonly PatientManager _patients;
    private readonly AnalyticsManager _analytics;

    public ReportManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
        _analytics = new AnalyticsManager(appointments, doctors, patients);
    }

    public IEnumerable<SpecialityReport> GetSpecialityStats()
    {
        Doctor[] allDoctors = _doctors.GetAll();
        Appointment[] allAppointments = _appointments.GetAll();

        return allDoctors
            .GroupBy(d => d.Speciality)
            .Select(g =>
            {
                int[] doctorIds = g.Select(d => d.Id).ToArray();
                Appointment[] ownAppointments = allAppointments
                    .Where(a => doctorIds.Contains(a.DoctorId))
                    .ToArray();

                return new SpecialityReport(
                    g.Key,
                    g.Count(),
                    ownAppointments.Length,
                    ownAppointments.Sum(a => a.GetCost()));
            })
            .OrderByDescending(r => r.TotalRevenue);
    }

    public string? FindBusiestDoctorName()
    {
        return _analytics.ComputeDoctorStats()
            .OrderByDescending(s => s.AppointmentCount)
            .Select(s => s.FullName)
            .FirstOrDefault();
    }

    public IEnumerable<string> GetPatientsWithMultipleVisits(int minVisits)
    {
        Patient[] allPatients = _patients.GetAll();

        return _appointments.GetAll()
            .GroupBy(a => a.PatientId)
            .Where(g => g.Count() >= minVisits)
            .Join(
                allPatients,
                g => g.Key,
                p => p.Id,
                (g, p) => p.FullName)
            .OrderBy(name => name);
    }

    public IEnumerable<DoctorStats> GetTopEarners(int n)
    {
        return _analytics.ComputeDoctorStats()
            .OrderByDescending(s => s.TotalRevenue)
            .Take(n);
    }

    public bool HasAnyUrgentAppointments()
    {
        return _appointments.GetAll().Any(a => a is UrgentAppointment);
    }

    public IEnumerable<Speciality> GetActiveSpecialities()
    {
        return _doctors.GetAll()
            .Select(d => d.Speciality)
            .Distinct()
            .OrderBy(s => s);
    }

    public IEnumerable<(int Year, int Month, decimal Total)> GetMonthlyRevenue()
    {
        return _appointments.GetAll()
            .GroupBy(a => new { a.ScheduledAt.Year, a.ScheduledAt.Month })
            .Select(g => (Year: g.Key.Year, Month: g.Key.Month, Total: g.Sum(a => a.GetCost())))
            .OrderBy(m => m.Year)
            .ThenBy(m => m.Month);
    }
}