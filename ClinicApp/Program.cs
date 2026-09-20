using ClinicApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

PatientManager patientManager = new PatientManager();
DoctorManager doctorManager = new DoctorManager();

DateTime today = DateTime.Today;

Patient p1 = new Patient("Іван", "Петренко", today.AddYears(-41), "A+", "0501234567");
Patient p2 = new Patient("Олена", "Коваль", today.AddYears(-33), "B-", "0672345678");
Patient p3 = new Patient("Максим", "Бойко", today.AddYears(-16), "O+", "0933456789");

patientManager.Add(p1);
patientManager.Add(p2);
patientManager.Add(p3);

Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");

doctorManager.Add(d1);
doctorManager.Add(d2);
doctorManager.Add(d3);

AppointmentManager appointmentManager = new AppointmentManager(patientManager, doctorManager);

DateTime dt1 = new DateTime(2026, 5, 9, 10, 0, 0);
if (appointmentManager.Book(1, 1, dt1, 30))
{
    Console.WriteLine($"Запис [1] створено: {p1.FullName} \u2192 {d1.FullName} о {dt1:dd.MM.yyyy HH:mm}");
}

appointmentManager.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
appointmentManager.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

appointmentManager.Book(99, 1, DateTime.Now);

Console.WriteLine();
Console.WriteLine("Майбутні записи:");
Appointment[] apps = new Appointment[]
{
    appointmentManager.GetByPatient(1)[0],
    appointmentManager.GetByPatient(2)[0],
    appointmentManager.GetByPatient(3)[0]
};
appointmentManager.DisplayList(apps);

Console.WriteLine();
if (appointmentManager.Cancel(1))
{
    Console.WriteLine("Запис [1] скасовано.");
}

Console.WriteLine();
Console.WriteLine("Записи пацієнта #2:");
appointmentManager.DisplayList(appointmentManager.GetByPatient(2));