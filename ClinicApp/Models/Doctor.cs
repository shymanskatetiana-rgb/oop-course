using System;
using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Doctor
{
    private static int _nextId = 1;
    private string _firstName = "";
    private string _lastName = "";
    private string _licenseNumber = "";
    private string _phone = "";

    public int Id { get; }
    public Speciality Speciality { get; set; } = Speciality.General;
    public WorkSchedule Schedule { get; set; }

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                throw new ArgumentException("Ім'я не може бути порожнім або довшим за 50 символів.", nameof(value));
            _firstName = value.Trim();
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                throw new ArgumentException("Прізвище не може бути порожнім або довшим за 50 символів.", nameof(value));
            _lastName = value.Trim();
        }
    }

    public string LicenseNumber
    {
        get => _licenseNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Номер ліцензії не може бути порожнім.",  nameof(value));
            _licenseNumber = value.Trim();
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 10 || !long.TryParse(value, out _))
                throw new ArgumentException("Номер телефону має складатися з 10 цифр.", nameof(value));
            _phone = value;
        }
    }

    public string FullName
    {
        get
        {
            return FirstName + " " + LastName;
        }
    }

    public int WorkHoursPerDay
    {
        get
        {
            return Schedule.HoursPerDay;
        }
    }

    public string WorkSchedule
    {
        get
        {
            return Schedule.Display;
        }
    }

    public bool IsAvailableNow
    {
        get
        {
            return Schedule.IsNow;
        }
    }

    public Doctor()
        : this("Невідомий", "Лікар", Speciality.General, "LIC-000", "0000000000")
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality)
        : this(firstName, lastName, speciality, "LIC-000", "0000000000")
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        Schedule = new WorkSchedule(8, 17);
    }

    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    public override string ToString()
    {
        string status;
        if (IsAvailableNow)
        {
            status = "доступний зараз";
        }
        else
        {
            status = "не в робочий час";
        }
        return $"[{Id}] {FullName} | {ClinicFormatter.FormatSpeciality(Speciality)} | {LicenseNumber} | Тел: {ClinicFormatter.FormatPhone(Phone)} | {Schedule} | {status}";
    }
}