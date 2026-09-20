using ClinicApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

DoctorManager doctorManager = new DoctorManager();

Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
d1.WorkEndHour = 16;
doctorManager.Add(d1);

Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
d2.WorkStartHour = 9;
d2.WorkEndHour = 18;
doctorManager.Add(d2);

Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");
doctorManager.Add(d3);

doctorManager.DisplayAll();
Console.WriteLine();
doctorManager.DisplayStats();

RunDoctorMenu(doctorManager);

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
        Console.WriteLine("0. Вихід");
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
                Console.Write("Година початку (8): ");
                if (int.TryParse(Console.ReadLine(), out int start)) doc.WorkStartHour = start;
                Console.Write("Година кінця (17): ");
                if (int.TryParse(Console.ReadLine(), out int end)) doc.WorkEndHour = end;
                dm.Add(doc);
                Console.WriteLine("Лікаря додано.");
                break;
            case "3":
                Console.Write("Спеціальність для пошуку: ");
                string searchSpec = Console.ReadLine() ?? "";
                var found = dm.FindBySpeciality(searchSpec);
                if (found.Length == 0)
                {
                    Console.WriteLine("Не знайдено.");
                }
                else
                {
                    foreach (var d in found) Console.WriteLine(d);
                }
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
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}