using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionManager
{
    private readonly string _sessionPath;

    public SessionManager(string sessionPath = "session.dat")
    {
        _sessionPath = sessionPath;
    }

    public bool Exists()
    {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic)
    {
        Patient[] patients = clinic.Patients.GetAll();

        using StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8);
        writer.WriteLine("[PATIENTS]");

        for (int i = 0; i < patients.Length; i++)
        {
            Patient p = patients[i];
            string date = p.DateOfBirth.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            writer.WriteLine($"{p.FirstName},{p.LastName},{date},{p.BloodType},{p.Phone}");
        }

        writer.WriteLine("[END]");
    }

    public int Load(Clinic clinic)
    {
        if (!File.Exists(_sessionPath)) return 0;

        string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
        string section = "";
        int added = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.Length == 0) continue;

            if (line.StartsWith("["))
            {
                section = line.ToUpper();
                continue;
            }

            if (section != "[PATIENTS]") continue;

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5) continue;

                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();
                DateTime dob = DateTime.ParseExact(parts[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3].Trim(), true);
                string phone = parts[4].Trim();

                if (PatientExists(clinic, firstName, lastName, dob)) continue;

                clinic.Patients.Add(new Patient(firstName, lastName, dob, bloodType, phone));
                added++;
            }
            catch (Exception)
            {
                // пошкоджений рядок - пропускаємо
            }
        }

        return added;
    }

    private bool PatientExists(Clinic clinic, string firstName, string lastName, DateTime dob)
    {
        Patient[] all = clinic.Patients.GetAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].FirstName == firstName
                && all[i].LastName == lastName
                && all[i].DateOfBirth.Date == dob.Date)
            {
                return true;
            }
        }
        return false;
    }
}
