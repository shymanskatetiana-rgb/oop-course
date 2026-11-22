using System;
using System.Globalization;
using System.IO;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public static class CsvImporter
{
    public static ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        ImportResult result = new ImportResult();

        if (!File.Exists(filePath))
        {
            result.AddError(0, "Файл не знайдено.");
            return result;
        }

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
        bool headerHandled = false;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.Length == 0) continue;

            if (!headerHandled)
            {
                headerHandled = true;
                if (line.StartsWith("FirstName", StringComparison.OrdinalIgnoreCase)) continue;
            }

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5)
                {
                    result.AddError(i + 1, $"Рядок має {parts.Length} поле замість 5.");
                    continue;
                }

                string firstName = parts[0].Trim();
                string lastName = parts[1].Trim();
                string dateStr = parts[2].Trim();
                string bloodStr = parts[3].Trim();
                string phone = parts[4].Trim();

                if (firstName.Length == 0) { result.AddError(i + 1, "Ім'я не може бути порожнім."); continue; }
                if (lastName.Length == 0) { result.AddError(i + 1, "Прізвище не може бути порожнім."); continue; }
                if (bloodStr.Length == 0) { result.AddError(i + 1, "Група крові не може бути порожньою."); continue; }
                if (phone.Length == 0) { result.AddError(i + 1, "Телефон не може бути порожнім."); continue; }

                DateTime dob = DateTime.ParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), bloodStr, true);

                Patient patient = new Patient(firstName, lastName, dob, bloodType, phone);
                clinic.Patients.Add(patient);
                result.AddSuccess();
            }
            catch (Exception ex)
            {
                result.AddError(i + 1, ex.Message);
            }
        }

        return result;
    }
}