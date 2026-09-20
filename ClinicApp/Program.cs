using ClinicApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Тест GrowablePatientManager ===");
Console.WriteLine("Додаємо пацієнтів одного за одним...");

GrowablePatientManager growableManager = new GrowablePatientManager();
Patient? targetP10 = null;

for (int i = 1; i <= 20; i++)
{
    Patient p = new Patient("Тест", $"Пацієнт{i}");
    growableManager.Add(p);

    if (i == 10)
    {
        targetP10 = p;
    }

    if (i <= 9 || i == 20)
    {
        Console.WriteLine($"  Додано [{i}]. Розмір: {growableManager.Count} / {growableManager.Capacity}");
    }
    else if (i == 10)
    {
        Console.WriteLine("  ...");
    }
}

Console.WriteLine();
Console.WriteLine("Тест пошуку:");

if (targetP10 != null && growableManager.FindById(targetP10.Id) != null)
{
    Console.WriteLine($"  FindById(10) \u2192 {targetP10.FullName}");
}
else
{
    Console.WriteLine("  FindById(10) \u2192 не знайдено");
}

Patient? p99 = growableManager.FindById(99);
if (p99 != null)
{
    Console.WriteLine($"  FindById(99) \u2192 {p99.FullName}");
}
else
{
    Console.WriteLine("  FindById(99) \u2192 не знайдено");
}

Console.WriteLine();
Console.WriteLine("Порівняння:");
Console.WriteLine($"  PatientManager:         100 місць (фіксовано)");
Console.WriteLine($"  GrowablePatientManager:  {growableManager.Capacity} місця (зросте при потребі)");