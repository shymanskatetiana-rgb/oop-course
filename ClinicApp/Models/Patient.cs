using System;
using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Patient
{
    private static int _nextId = 1;
    private string _firstName = "";
    private string _lastName = "";
    private DateTime _dateOfBirth;
    private string _phone = "";
    private string _email = "";

    public int Id { get; }
    public BloodType BloodType { get; set; } = BloodType.Unknown;

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                throw new ArgumentException("Ім'я не може бути порожнім або довшим за 50 символів.", nameof(FirstName));
            _firstName = value.Trim();
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
                throw new ArgumentException("Прізвище не може бути порожнім або довшим за 50 символів.", nameof(LastName));
            _lastName = value.Trim();
        }
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value > DateTime.Today || value.Year < 1900)
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), "Дата народження не може бути в майбутньому або раніше 1900 року.");
            _dateOfBirth = value;
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 10)
                throw new ArgumentException("Номер телефону має складатися рівно з 10 символів.", nameof(Phone));

            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                    throw new ArgumentException("Номер телефону має містити лише цифри.", nameof(Phone));
            }

            _phone = value;
        }
    }

    public string Email
    {
        get => _email;
        set => _email = value;
    }

    public string FullName
    {
        get { return FirstName + " " + LastName; }
    }

    public int Age
    {
        get
        {
            DateTime today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }

    public bool IsAdult
    {
        get { return Age >= 18; }
    }

    public Patient(string firstName, string lastName, DateTime dateOfBirth, BloodType bloodType, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        BloodType = bloodType;
        Phone = phone;
        Email = "";

        Id = _nextId++;
    }

    public Patient(string firstName, string lastName)
        : this(firstName, lastName, DateTime.Today.AddYears(-26), BloodType.Unknown, "0000000000")
    {
    }

    public Patient()
        : this("Невідомий", "Пацієнт", DateTime.Today.AddYears(-26), BloodType.Unknown, "0000000000")
    {
    }

    public string GetAgeCategory()
    {
        if (Age < 18) return "дитина";
        else if (Age < 60) return "дорослий";
        else return "літній";
    }

    public override string ToString()
    {
        return $"[{Id}] {FullName} | Вік: {ClinicFormatter.FormatAge(Age)} ({GetAgeCategory()}) | Кров: {ClinicFormatter.FormatBloodType(BloodType)} | Тел: {ClinicFormatter.FormatPhone(Phone)}";
    }
}