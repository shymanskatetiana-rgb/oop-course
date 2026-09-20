using ClinicApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

PatientManager manager = new PatientManager();
DateTime today = DateTime.Today;

manager.Add(new Patient("Іван", "Петренко", today.AddYears(-41), "A+", "0501234567"));
manager.Add(new Patient("Олена", "Коваль", today.AddYears(-33), "B-", "0672345678"));
manager.Add(new Patient("Максим", "Бойко", today.AddYears(-16), "O+", "0933456789"));
manager.Add(new Patient("Марія", "Ткач"));

Console.WriteLine();
manager.DisplayAll();
Console.WriteLine();
manager.DisplayStats();

RunPatientMenu(manager);

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
        Console.WriteLine("0. Вихід");
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
                break;
            case "3":
                Console.Write("Введіть ім'я або прізвище: ");
                string search = Console.ReadLine() ?? "";
                var found = pm.FindByName(search);
                if (found.Length == 0)
                {
                    Console.WriteLine("Нікого не знайдено.");
                }
                else
                {
                    foreach (var p in found) Console.WriteLine(p);
                }
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
            default:
                Console.WriteLine("Невірний вибір.");
                break;
        }
    }
}