using ClinicApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

DateTime today = DateTime.Today;

Patient p1 = new Patient("Олександр", "Грицько", today.AddYears(-41), "A+", "0501234567");
Patient p2 = new Patient("Ольга", "Коваленко", today.AddYears(-33), "B-", "0672345678");
Patient p3 = new Patient("Олег", "Локран", today.AddYears(-16), "O+", "0933456789");

Patient p4 = new Patient();


Patient p5 = new Patient("Тетяна", "Шиманська");

Console.WriteLine(p1);
Console.WriteLine(p2);
Console.WriteLine(p3);
Console.WriteLine(p4);
Console.WriteLine(p5);