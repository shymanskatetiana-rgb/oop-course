using System;
using System.Globalization;
using System.Threading;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

Clinic clinic = new Clinic("Медична Клініка");
DateTime today = DateTime.Today;

clinic.Patients.Add(new Patient("Іван", "Петренко", today.AddYears(-41), BloodType.APositive, "0501234567"));
clinic.Patients.Add(new Patient("Олена", "Коваль", today.AddYears(-33), BloodType.BNegative, "0672345678"));
clinic.Patients.Add(new Patient("Максим", "Бойко", today.AddYears(-16), BloodType.OPositive, "0933456789"));
clinic.Patients.Add(new Patient("Марія", "Ткач"));

clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567"));
clinic.Doctors.Add(new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678"));
clinic.Doctors.Add(new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789"));

clinic.Appointments.Book(1, 1, DateTime.Today.AddHours(10), 30);
clinic.Appointments.Book(2, 2, DateTime.Today.AddHours(11), 45);

WorkSchedule morning = new WorkSchedule(8, 16);
WorkSchedule copy = morning;
copy = new WorkSchedule(9, 18);
Console.WriteLine(morning);
Console.WriteLine(copy);
Console.WriteLine(morning.IsNow);

Doctor[] cardiologists = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
Doctor[] foundDocs = clinic.Doctors.FindBySpeciality("кардіо");
Appointment[] appToday = clinic.Appointments.GetByDate(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

if (clinic.Patients.TryFindById(1, out Patient patient))
{
    Console.WriteLine($"Знайдено: {patient.FullName}");
}
else
{
    Console.WriteLine("Пацієнта не знайдено.");
}

string unknownName = clinic.Patients.FindById(99)?.FullName ?? "не знайдено";
Console.WriteLine(unknownName);

// Пацієнт #1 — найбагатша картка
clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", true));
clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-5), "J06.9", "Гострий ринофарингіт", false));
clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today.AddDays(-10), "Гемоглобін", 145, "г/л", "120–160", true));
clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today.AddDays(-8), "Холестерин", 6.2, "ммоль/л", "< 5.2", false));
clinic.MedicalRecords.Add(new Prescription(1, 1, DateTime.Today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));

// Пацієнт #2 — завершений рецепт
clinic.MedicalRecords.Add(new Prescription(2, 2, DateTime.Today.AddDays(-40), "Аспірин", "500 мг", 10));

// Пацієнт #3 — один запис (для перевірки, що порожніх розділів немає)
clinic.MedicalRecords.Add(new Diagnosis(3, 3, DateTime.Today.AddDays(-3), "K29.7", "Гастрит", false));

RunMainMenu(clinic);

static void RunMainMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n=== Головне меню Клініки ===");
        Console.WriteLine("1. Пацієнти");
        Console.WriteLine("2. Лікарі");
        Console.WriteLine("3. Записи на прийом");
        Console.WriteLine("4. Медична картка");
        Console.WriteLine("5. Загальний звіт клініки");
        Console.WriteLine("0. Вихід");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0" || string.IsNullOrEmpty(choice)) break;

        switch (choice)
        {
            case "1":
                RunPatientMenu(clinic.Patients);
                break;
            case "2":
                RunDoctorMenu(clinic.Doctors);
                break;
            case "3":
                RunAppointmentMenu(clinic.Appointments, clinic.Patients, clinic.Doctors);
                break;
            case "4":
                RunMedicalRecordsMenu(clinic);
                break;
            case "5":
                clinic.GenerateReport();
                break;
        }
    }
}

static void RunPatientMenu(PatientManager manager)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Пацієнти» ---");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати пацієнта");
        Console.WriteLine("3. Пошук за ім'ям");
        Console.WriteLine("4. Видалити за ID");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0" || string.IsNullOrEmpty(choice)) break;

        switch (choice)
        {
            case "1":
                manager.DisplayAll();
                break;
            case "2":
                Console.Write("Ім'я: ");
                string fn = Console.ReadLine() ?? "";
                Console.Write("Прізвище: ");
                string ln = Console.ReadLine() ?? "";
                Console.Write("Номер телефону (10 цифр): ");
                string phone = Console.ReadLine() ?? "";
                Console.WriteLine("Оберіть групу крові (0 - Unknown, 1 - APositive, 2 - ANegative, 3 - BPositive, 4 - BNegative, 5 - ABPositive, 6 - ABNegative, 7 - OPositive, 8 - ONegative): ");
                Console.Write("Номер: ");
                int.TryParse(Console.ReadLine(), out int btIndex);
                BloodType bt = (BloodType)btIndex;

                try
                {
                    manager.Add(new Patient(fn, ln, DateTime.Today.AddYears(-20), bt, phone));
                    Console.WriteLine("Пацієнта успішно додано!");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"Помилка у межах даних: {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Помилка введення: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Несподівана помилка: {ex.Message}");
                }
                break;
            case "3":
                Console.Write("Пошуковий запит: ");
                string q = Console.ReadLine() ?? "";
                var res = manager.FindByName(q);
                if (res.Length == 0) Console.WriteLine("Нікого не знайдено.");
                else foreach (var p in res) Console.WriteLine(p);
                break;
            case "4":
                Console.Write("ID для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    if (manager.Remove(id)) Console.WriteLine("Видалено.");
                    else Console.WriteLine("Не знайдено.");
                }
                break;
            case "5":
                manager.DisplayStats();
                break;
        }
    }
}

static void RunDoctorMenu(DoctorManager manager)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Лікарі» ---");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Пошук за спеціальністю");
        Console.WriteLine("3. Додати лікаря");
        Console.WriteLine("4. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0" || string.IsNullOrEmpty(choice)) break;

        switch (choice)
        {
            case "1":
                manager.DisplayAll();
                break;
            case "2":
                Console.Write("Спеціальність для пошуку: ");
                string spec = Console.ReadLine() ?? "";
                var docs = manager.FindBySpeciality(spec);
                if (docs.Length == 0) Console.WriteLine("Лікарів не знайдено.");
                else foreach (var d in docs) Console.WriteLine(d);
                break;
            case "3":
                Console.Write("Ім'я: ");
                string dfn = Console.ReadLine() ?? "";
                Console.Write("Прізвище: ");
                string dln = Console.ReadLine() ?? "";
                Console.Write("Номер ліцензії: ");
                string lic = Console.ReadLine() ?? "";
                Console.Write("Телефон (10 цифр): ");
                string dphone = Console.ReadLine() ?? "";
                Console.WriteLine("Оберіть спеціальність (0 - General, 1 - Cardiology, 2 - Neurology, 3 - Pediatrics, 4 - Surgery, 5 - Orthopedics, 6 - Dermatology, 7 - Emergency): ");
                Console.Write("Номер: ");
                int.TryParse(Console.ReadLine(), out int specIdx);
                Speciality newSpec = (Speciality)specIdx;

                Console.Write("Година початку роботи (0-23): ");
                int.TryParse(Console.ReadLine(), out int startH);
                Console.Write("Година завершення роботи (1-24): ");
                int.TryParse(Console.ReadLine(), out int endH);

                try
                {
                    WorkSchedule schedule = new WorkSchedule(startH, endH);
                    Doctor newDoctor = new Doctor(dfn, dln, newSpec, lic, dphone);
                    newDoctor.Schedule = schedule;
                    manager.Add(newDoctor);
                    Console.WriteLine("Лікаря успішно додано!");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"Помилка діапазону: {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Помилка введення: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Несподівана помилка: {ex.Message}");
                }
                break;
            case "4":
                manager.DisplayStats();
                break;
        }
    }
}

static void RunAppointmentMenu(AppointmentManager appManager, PatientManager pManager, DoctorManager dManager)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Записи» ---");
        Console.WriteLine("1. Майбутні записи");
        Console.WriteLine("2. Записи на сьогодні");
        Console.WriteLine("3. Створити запис");
        Console.WriteLine("4. Скасувати запис");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0" || string.IsNullOrEmpty(choice)) break;

        switch (choice)
        {
            case "1":
                appManager.DisplayList(appManager.GetUpcoming());
                break;
            case "2":
                appManager.DisplayList(appManager.GetByDate(DateTime.Today));
                break;
            case "3":
                Console.WriteLine("\nСписок пацієнтів:");
                pManager.DisplayAll();
                Console.Write("ID пацієнта: ");
                int.TryParse(Console.ReadLine(), out int pId);

                Console.WriteLine("\nСписок лікарів:");
                dManager.DisplayAll();
                Console.Write("ID лікаря: ");
                int.TryParse(Console.ReadLine(), out int dId);

                Console.Write("Година прийому сьогодні (0-23): ");
                int.TryParse(Console.ReadLine(), out int h);

                Console.Write("Тривалість у хвилинах (наприклад 30): ");
                int.TryParse(Console.ReadLine(), out int duration);

                try
                {
                    appManager.Book(pId, dId, DateTime.Today.AddHours(h), duration);
                    Console.WriteLine("Запис успішно створено!");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"Помилка часу або тривалості: {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Помилка даних запису: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Несподівана помилка: {ex.Message}");
                }
                break;
            case "4":
                Console.Write("ID запису для скасування: ");
                if (int.TryParse(Console.ReadLine(), out int appId))
                {
                    Console.Write("Причина: ");
                    string reason = Console.ReadLine() ?? "";
                    if (appManager.Cancel(appId, reason)) Console.WriteLine("Запис успішно скасовано.");
                    else Console.WriteLine("Не вдалося скасувати запис.");
                }
                break;
        }
    }
}

static void RunMedicalRecordsMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Медична картка» ---");
        Console.WriteLine("1. Картка пацієнта (зведення)");
        Console.WriteLine("2. Усі записи пацієнта");
        Console.WriteLine("3. Додати діагноз");
        Console.WriteLine("4. Додати аналіз");
        Console.WriteLine("5. Додати рецепт");
        Console.WriteLine("6. Записи лікаря");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0" || string.IsNullOrEmpty(choice)) break;

        switch (choice)
        {
            case "1":
                Console.Write("ID пацієнта: ");
                if (int.TryParse(Console.ReadLine(), out int id1))
                {
                    clinic.MedicalRecords.DisplayPatientSummary(id1);
                }
                else
                {
                    Console.WriteLine("Помилка: введіть ціле число.");
                }
                break;

            case "2":
                Console.Write("ID пацієнта: ");
                if (int.TryParse(Console.ReadLine(), out int id2))
                {
                    MedicalRecord[] recs = clinic.MedicalRecords.GetByPatient(id2);
                    clinic.MedicalRecords.DisplayList(recs);
                }
                else
                {
                    Console.WriteLine("Помилка: введіть ціле число.");
                }
                break;

            case "3":
                AddDiagnosis(clinic);
                break;

            case "4":
                AddLabResult(clinic);
                break;

            case "5":
                AddPrescription(clinic);
                break;

            case "6":
                Console.Write("ID лікаря: ");
                if (int.TryParse(Console.ReadLine(), out int id6))
                {
                    MedicalRecord[] recs = clinic.MedicalRecords.GetByDoctor(id6);
                    clinic.MedicalRecords.DisplayList(recs);
                }
                else
                {
                    Console.WriteLine("Помилка: введіть ціле число.");
                }
                break;
        }
    }
}

static void AddDiagnosis(Clinic clinic)
{
    Console.Write("ID пацієнта: ");
    if (!int.TryParse(Console.ReadLine(), out int pId))
    {
        Console.WriteLine("Помилка: введіть ціле число.");
        return;
    }
    if (clinic.Patients.FindById(pId) == null)
    {
        Console.WriteLine($"Пацієнта #{pId} не знайдено.");
        return;
    }

    Console.Write("ID лікаря: ");
    if (!int.TryParse(Console.ReadLine(), out int dId))
    {
        Console.WriteLine("Помилка: введіть ціле число.");
        return;
    }
    if (clinic.Doctors.FindById(dId) == null)
    {
        Console.WriteLine($"Лікаря #{dId} не знайдено.");
        return;
    }

    Console.Write("Код діагнозу (напр. J06.9): ");
    string code = Console.ReadLine() ?? "";

    Console.Write("Опис: ");
    string desc = Console.ReadLine() ?? "";

    Console.Write("Хронічне? (1=так, 0=ні): ");
    int.TryParse(Console.ReadLine(), out int chronicInt);
    bool isChronic = chronicInt == 1;

    try
    {
        Diagnosis d = new Diagnosis(pId, dId, DateTime.Today, code, desc, isChronic);
        clinic.MedicalRecords.Add(d);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
}

static void AddLabResult(Clinic clinic)
{
    Console.Write("ID пацієнта: ");
    if (!int.TryParse(Console.ReadLine(), out int pId))
    {
        Console.WriteLine("Помилка: введіть ціле число.");
        return;
    }
    if (clinic.Patients.FindById(pId) == null)
    {
        Console.WriteLine($"Пацієнта #{pId} не знайдено.");
        return;
    }

    Console.Write("ID лікаря: ");
    if (!int.TryParse(Console.ReadLine(), out int dId))
    {
        Console.WriteLine("Помилка: введіть ціле число.");
        return;
    }
    if (clinic.Doctors.FindById(dId) == null)
    {
        Console.WriteLine($"Лікаря #{dId} не знайдено.");
        return;
    }

    Console.Write("Назва аналізу: ");
    string testName = Console.ReadLine() ?? "";

    // ОСЬ ТУТ — парсинг double з InvariantCulture
    Console.Write("Значення (число з крапкою): ");
    string valueStr = Console.ReadLine() ?? "";
    if (!double.TryParse(valueStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
    {
        Console.WriteLine("Помилка: некоректне число. Використовуйте крапку: 6.2");
        return;
    }

    Console.Write("Одиниці виміру: ");
    string unit = Console.ReadLine() ?? "";

    Console.Write("Норма (напр. 4.0-9.0): ");
    string refRange = Console.ReadLine() ?? "";

    Console.Write("В нормі? (1=так, 0=ні): ");
    int.TryParse(Console.ReadLine(), out int normalInt);
    bool isNormal = normalInt == 1;

    try
    {
        LabResult lr = new LabResult(pId, dId, DateTime.Today, testName, value, unit, refRange, isNormal);
        clinic.MedicalRecords.Add(lr);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
}

static void AddPrescription(Clinic clinic)
{
    Console.Write("ID пацієнта: ");
    if (!int.TryParse(Console.ReadLine(), out int pId))
    {
        Console.WriteLine("Помилка: введіть ціле число.");
        return;
    }
    if (clinic.Patients.FindById(pId) == null)
    {
        Console.WriteLine($"Пацієнта #{pId} не знайдено.");
        return;
    }

    Console.Write("ID лікаря: ");
    if (!int.TryParse(Console.ReadLine(), out int dId))
    {
        Console.WriteLine("Помилка: введіть ціле число.");
        return;
    }
    if (clinic.Doctors.FindById(dId) == null)
    {
        Console.WriteLine($"Лікаря #{dId} не знайдено.");
        return;
    }

    Console.Write("Препарат: ");
    string med = Console.ReadLine() ?? "";

    Console.Write("Дозування (напр. 10 мг): ");
    string dosage = Console.ReadLine() ?? "";

    Console.Write("Кількість днів: ");
    if (!int.TryParse(Console.ReadLine(), out int days))
    {
        Console.WriteLine("Помилка: введіть ціле число.");
        return;
    }

    Console.Write("Інструкція (Enter — пропустити): ");
    string instructions = Console.ReadLine() ?? "";

    try
    {
        Prescription p = new Prescription(pId, dId, DateTime.Today, med, dosage, days, instructions);
        clinic.MedicalRecords.Add(p);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
}
