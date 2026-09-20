using ClinicApp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
d1.WorkEndHour = 16;

Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
d2.WorkStartHour = 9;
d2.WorkEndHour = 18; 

Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");

Console.WriteLine(d1);
Console.WriteLine(d2);
Console.WriteLine(d3);