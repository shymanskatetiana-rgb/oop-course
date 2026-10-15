using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Models;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; set; }
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }
    public MedicalRecordManager MedicalRecords { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
        MedicalRecords = new MedicalRecordManager();
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"=== Розклад на {date:dd.MM.yyyy} ===");
        Appointment[] apps = Appointments.GetByDate(date);
        Appointments.DisplayList(apps);
    }

    public void GenerateReport()
    {
        Doctor[] allDoctors = Doctors.GetAll();
        Appointment[] upcoming = Appointments.GetUpcoming();

        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine($"║  Звіт — {Name,-37}║");
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine($"║  Пацієнтів:          {Patients.Count,-24}║");
        Console.WriteLine($"║  Лікарів:            {Doctors.Count,-24}║");
        Console.WriteLine($"║  Майбутніх записів:  {upcoming.Length,-24}║");
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine("║  Навантаження лікарів (майбутні записи):     ║");

        for (int i = 0; i < allDoctors.Length; i++)
        {
            int doctorAppsCount = 0;
            for (int j = 0; j < upcoming.Length; j++)
            {
                if (upcoming[j].DoctorId == allDoctors[i].Id)
                {
                    doctorAppsCount++;
                }
            }

            string line = $"    {allDoctors[i].FullName} ({allDoctors[i].Speciality}): {doctorAppsCount} записів";
            Console.WriteLine($"║{line,-46}║");
        }

        Console.WriteLine("╚══════════════════════════════════════════════╝");
    }


}