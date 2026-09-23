using ClinicApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

PatientManager pm = new PatientManager();
DateTime today = DateTime.Today;

pm.Add(new Patient("Іван", "Петренко", today.AddYears(-41), "A+", "0501234567"));
pm.Add(new Patient("Олена", "Коваль", today.AddYears(-33), "B-", "0672345678"));
pm.Add(new Patient("Максим", "Бойко", today.AddYears(-16), "O+", "0933456789"));
pm.Add(new Patient("Марія", "Ткач"));

Console.WriteLine();
pm.DisplayAll();

Console.WriteLine();
Console.WriteLine("Пошук 'ов':");
Patient[] found = pm.FindByName("ов");
foreach (Patient p in found)
{
    Console.WriteLine(p);
}

Console.WriteLine();
pm.DisplayStats();

Console.WriteLine();
Console.WriteLine("Видалення ID 2:");
pm.Remove(2);
pm.DisplayAll();

RunPatientMenu(pm);

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
        Console.WriteLine("0. Вихід");
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
                manager.Add(new Patient(fn, ln));
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