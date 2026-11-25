using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; set; }
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }
    public MedicalRecordManager MedicalRecords { get; }
    public BillingManager Billing {  get; }
    public AnalyticsManager Analytics { get; }
    public WaitingQueue<Patient> WaitingRoom { get; }
    public TreatmentPlanManager TreatmentPlans { get; }
    public ClinicLogger Logger { get; }
    public ClinicExporter Exporter { get; }
    public SessionManager Session {  get; }
    public PatientPassportWriter Passport { get; }
    public SessionEventTracker Tracker { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
        MedicalRecords = new MedicalRecordManager();
        Billing = new BillingManager(Appointments);
        Analytics = new AnalyticsManager(Appointments, Doctors, Patients);
        WaitingRoom = new WaitingQueue<Patient>();
        TreatmentPlans = new TreatmentPlanManager();
        Logger = new ClinicLogger();
        Exporter = new ClinicExporter(this);
        Session = new SessionManager();
        Passport = new PatientPassportWriter(this);
        Tracker = new SessionEventTracker(this);
        SubscribeEvents();
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

    private void SubscribeEvents()
    {
        Appointments.AppointmentBooked += Logger.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Logger.OnUrgentBooked;
        Appointments.AppointmentCancelled += Logger.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Logger.OnAppointmentCompleted;
        Patients.PatientAdded += Logger.OnPatientAdded;
        Billing.PaymentReceived += Logger.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Logger.OnPlanCompleted;
        Patients.PatientAdded += Passport.OnPatientAdded;
        Appointments.AppointmentCompleted += Passport.OnAppointmentCompleted;
        TreatmentPlans.PlanCompleted += Passport.OnPlanCompleted;
        Patients.PatientAdded += Tracker.OnPatientAdded;
        Appointments.AppointmentBooked += Tracker.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Tracker.OnUrgentBooked;
        Appointments.AppointmentCancelled += Tracker.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Tracker.OnAppointmentCompleted;
        Billing.PaymentReceived += Tracker.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Tracker.OnPlanCompleted;
    }

}