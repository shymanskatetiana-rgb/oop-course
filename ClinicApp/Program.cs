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