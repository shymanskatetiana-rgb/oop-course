using System;
using System.Collections.Generic;
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

        for (int i = 0; i < allDoctors.Length; i++)
        {
            Doctor doc = allDoctors[i];
            int count = 0;
            decimal revenue = 0;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < allAppointments.Length; j++)
            {
                if (allAppointments[j].DoctorId == doc.Id)
                {
                    count++;
                    revenue += allAppointments[j].GetCost();

                    if (allAppointments[j].ScheduledAt > lastDate)
                    {
                        lastDate = allAppointments[j].ScheduledAt;
                    }
                }
            }

            yield return new DoctorStats(doc.Id, doc.FullName, count, revenue, lastDate);
        }
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] allPatients = _patients.GetAll();
        Appointment[] allAppointments = _appointments.GetAll();

        for (int i = 0; i < allPatients.Length; i++)
        {
            Patient pat = allPatients[i];
            int count = 0;
            decimal totalSpent = 0;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < allAppointments.Length; j++)
            {
                if (allAppointments[j].PatientId == pat.Id)
                {
                    count++;
                    totalSpent += allAppointments[j].GetCost();

                    if (allAppointments[j].ScheduledAt > lastDate)
                    {
                        lastDate = allAppointments[j].ScheduledAt;
                    }
                }
            }

            yield return new PatientStats(pat.Id, pat.FullName, count, totalSpent, lastDate);
        }
    }
}