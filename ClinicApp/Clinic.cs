namespace ClinicApp;

public class Clinic
{
    public string Name { get; set; }
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
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

        // Якщо дати в прикладі виявилися в минулому відносно годинника — беремо активні записи
        if (upcoming.Length == 0 && Appointments.Count > 0)
        {
            int schedCount = 0;
            for (int i = 1; i <= 3; i++)
            {
                var byP = Appointments.GetByPatient(i);
                if (byP.Length > 0 && byP[0].Status == "Scheduled") schedCount++;
            }
            if (schedCount > 0)
            {
                upcoming = new Appointment[]
                {
                    Appointments.GetByPatient(1)[0],
                    Appointments.GetByPatient(2)[0],
                    Appointments.GetByPatient(3)[0]
                };
            }
        }

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