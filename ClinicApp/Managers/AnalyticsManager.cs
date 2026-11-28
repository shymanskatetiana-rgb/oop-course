using System;
using System.Collections.Generic;
using System.Linq;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private readonly AppointmentManager _appointments;
    private readonly DoctorManager _doctors;
    private readonly PatientManager _patients;

    public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Doctor[] allDoctors = _doctors.GetAll();
        Appointment[] allAppointments = _appointments.GetAll();

        return allDoctors.Select(d =>
        {
            Appointment[] ownAppointments = allAppointments
                .Where(a => a.DoctorId == d.Id)
                .ToArray();

            return new DoctorStats(
                d.Id,
                d.FullName,
                ownAppointments.Count(),
                ownAppointments.Sum(a => a.GetCost()),
                ownAppointments.Any() ? ownAppointments.Max(a => a.ScheduledAt) : DateTime.MinValue);
        });
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] allPatients = _patients.GetAll();
        Appointment[] allAppointments = _appointments.GetAll();

        return allPatients.Select(p =>
        {
            Appointment[] ownAppointments = allAppointments
                .Where(a => a.PatientId == p.Id)
                .ToArray();

            return new PatientStats(
                p.Id,
                p.FullName,
                ownAppointments.Count(),
                ownAppointments.Sum(a => a.GetCost()),
                ownAppointments.Any() ? ownAppointments.Max(a => a.ScheduledAt) : DateTime.MinValue);
        });
    }
}