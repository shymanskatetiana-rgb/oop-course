using ClinicApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Clinic clinic = new Clinic("Медична Клініка");

DateTime today = DateTime.Today;

// Додавання 4 пацієнтів
clinic.Patients.Add(new Patient("Іван", "Петренко", today.AddYears(-41), "A+", "0501234567"));
clinic.Patients.Add(new Patient("Олена", "Коваль", today.AddYears(-33), "B-", "0672345678"));
clinic.Patients.Add(new Patient("Максим", "Бойко", today.AddYears(-16), "O+", "0933456789"));
clinic.Patients.Add(new Patient("Марія", "Ткач"));

// Додавання 3 лікарів
Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");

clinic.Doctors.Add(d1);
clinic.Doctors.Add(d2);
clinic.Doctors.Add(d3);

// Бронювання записів
DateTime date1 = new DateTime(2026, 5, 9, 10, 0, 0);
DateTime date2 = new DateTime(2026, 5, 9, 11, 0, 0);
DateTime date3 = new DateTime(2026, 5, 10, 9, 0, 0);

clinic.Appointments.Book(1, 1, date1, 30);
clinic.Appointments.Book(2, 2, date2, 45);
clinic.Appointments.Book(3, 3, date3, 20);

// Демонстрація за зразком
clinic.DisplaySchedule(new DateTime(2026, 5, 9));
Console.WriteLine();
clinic.GenerateReport();

// Головне інтерактивне меню
RunMainMenu(clinic);

static void RunMainMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n=== ГОЛОВНЕ МЕНЮ КЛІНІКИ ===");
        Console.WriteLine("1. Меню «Пацієнти»");
        Console.WriteLine("2. Меню «Лікарі»");
        Console.WriteLine("3. Меню «Записи»");
        Console.WriteLine("4. Розклад на дату");
        Console.WriteLine("5. Звіт клініки");
        Console.WriteLine("0. Вихід");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0") break;

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
                Console.Write("Введіть дату (рррр-мм-дд): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime dt))
                {
                    clinic.DisplaySchedule(dt);
                }
                break;
            case "5":
                clinic.GenerateReport();
                break;
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}

static void RunPatientMenu(PatientManager pm)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Пацієнти» ---");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати пацієнта");
        Console.WriteLine("3. Пошук за ім'ям");
        Console.WriteLine("4. Видалити за Id");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0") break;

        switch (choice)
        {
            case "1":
                pm.DisplayAll();
                break;
            case "2":
                Console.Write("Ім'я: ");
                string fName = Console.ReadLine() ?? "";
                Console.Write("Прізвище: ");
                string lName = Console.ReadLine() ?? "";
                Console.Write("Група крові: ");
                string blood = Console.ReadLine() ?? "";
                Console.Write("Телефон: ");
                string phone = Console.ReadLine() ?? "";
                pm.Add(new Patient(fName, lName, DateTime.Today.AddYears(-20), blood, phone));
                Console.WriteLine("Пацієнта додано.");
                break;
            case "3":
                Console.Write("Ім'я або прізвище: ");
                string search = Console.ReadLine() ?? "";
                var found = pm.FindByName(search);
                if (found.Length == 0) Console.WriteLine("Нікого не знайдено.");
                else foreach (var p in found) Console.WriteLine(p);
                break;
            case "4":
                Console.Write("Id для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int remId))
                {
                    if (pm.Remove(remId)) Console.WriteLine("Пацієнта видалено.");
                    else Console.WriteLine("Пацієнта не знайдено.");
                }
                break;
            case "5":
                pm.DisplayStats();
                break;
        }
    }
}

static void RunDoctorMenu(DoctorManager dm)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Лікарі» ---");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати лікаря");
        Console.WriteLine("3. Пошук за спеціальністю");
        Console.WriteLine("4. Видалити лікаря");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0") break;

        switch (choice)
        {
            case "1":
                dm.DisplayAll();
                break;
            case "2":
                Console.Write("Ім'я: ");
                string fName = Console.ReadLine() ?? "";
                Console.Write("Прізвище: ");
                string lName = Console.ReadLine() ?? "";
                Console.Write("Спеціальність: ");
                string spec = Console.ReadLine() ?? "";
                Console.Write("Номер ліцензії: ");
                string lic = Console.ReadLine() ?? "";
                Console.Write("Телефон: ");
                string phone = Console.ReadLine() ?? "";
                Doctor doc = new Doctor(fName, lName, spec, lic, phone);
                dm.Add(doc);
                Console.WriteLine("Лікаря додано.");
                break;
            case "3":
                Console.Write("Спеціальність: ");
                string searchSpec = Console.ReadLine() ?? "";
                var found = dm.FindBySpeciality(searchSpec);
                if (found.Length == 0) Console.WriteLine("Не знайдено.");
                else foreach (var d in found) Console.WriteLine(d);
                break;
            case "4":
                Console.Write("Id для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int remId))
                {
                    if (dm.Remove(remId)) Console.WriteLine("Видалено.");
                    else Console.WriteLine("Не знайдено.");
                }
                break;
            case "5":
                dm.DisplayStats();
                break;
        }
    }
}

static void RunAppointmentMenu(AppointmentManager am, PatientManager pm, DoctorManager dm)
{
    while (true)
    {
        Console.WriteLine("\n--- Меню «Записи» ---");
        Console.WriteLine("1. Записати пацієнта");
        Console.WriteLine("2. Скасувати запис");
        Console.WriteLine("3. Завершити прийом");
        Console.WriteLine("4. Записи за пацієнтом");
        Console.WriteLine("5. Записи за лікарем");
        Console.WriteLine("6. Записи на дату");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        string? choice = Console.ReadLine();
        if (choice == "0") break;

        switch (choice)
        {
            case "1":
                Console.Write("ID пацієнта: ");
                int.TryParse(Console.ReadLine(), out int pId);
                Console.Write("ID лікаря: ");
                int.TryParse(Console.ReadLine(), out int dId);
                Console.Write("Дата і час (рррр-мм-дд гг:хх): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime dt)) dt = DateTime.Now.AddHours(2);
                Console.Write("Тривалість у хвилинах (30): ");
                if (!int.TryParse(Console.ReadLine(), out int duration)) duration = 30;
                am.Book(pId, dId, dt, duration);
                break;
            case "2":
                Console.Write("ID запису для скасування: ");
                if (int.TryParse(Console.ReadLine(), out int cancelId))
                {
                    Console.Write("Причина скасування: ");
                    string reason = Console.ReadLine() ?? "";
                    am.Cancel(cancelId, reason);
                }
                break;
            case "3":
                Console.Write("ID запису для завершення: ");
                if (int.TryParse(Console.ReadLine(), out int compId))
                {
                    am.Complete(compId);
                }
                break;
            case "4":
                Console.Write("ID пацієнта: ");
                if (int.TryParse(Console.ReadLine(), out int sPId))
                {
                    am.DisplayList(am.GetByPatient(sPId));
                }
                break;
            case "5":
                Console.Write("ID лікаря: ");
                if (int.TryParse(Console.ReadLine(), out int sDId))
                {
                    am.DisplayList(am.GetByDoctor(sDId));
                }
                break;
            case "6":
                Console.Write("Дата (рррр-мм-дд): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime sDate))
                {
                    am.DisplayList(am.GetByDate(sDate));
                }
                break;
        }
    }
}