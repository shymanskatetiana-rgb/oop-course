using System;
using System.Collections.Generic;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class PatientManager
{
    private List<Patient> _patients = new List<Patient>();

    public int Count
    {
        get
        {
            return _patients.Count;
        }
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> result = new List<Patient>();
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                result.Add(_patients[i]);
            }
        }
        return result.ToArray();
    }

    public bool TryFindById(int id, out Patient patient)
    {
        Patient? found = FindById(id);
        if (found != null)
        {
            patient = found;
            return true;
        }

        patient = null!;
        return false;
    }

    public Patient? this[int index]
    {
        get
        {
            if (index < 0 || index >= _patients.Count)
            {
                return null;
            }
            return _patients[index];
        }
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public Patient[] FindByName(string query)
    {
        string q = (query ?? "").Trim().ToLower();
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(q) || _patients[i].LastName.ToLower().Contains(q))
            {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public bool Remove(int id)
    {
        Patient? patient = FindById(id);
        if (patient != null)
        {
            _patients.Remove(patient);
            return true;
        }
        return false;
    }

    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("порожній список");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_patients.Count}) ===");
        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
    }

    public void DisplayStats()
    {
        Console.WriteLine("=== Статистика пацієнтів ===");
        if (_patients.Count == 0)
        {
            Console.WriteLine("порожній список");
            return;
        }

        int totalAge = 0;
        int minIdx = 0;
        int maxIdx = 0;
        int adultsCount = 0;

        for (int i = 0; i < _patients.Count; i++)
        {
            int age = _patients[i].Age;
            totalAge += age;

            if (age < _patients[minIdx].Age)
            {
                minIdx = i;
            }

            if (age > _patients[maxIdx].Age)
            {
                maxIdx = i;
            }

            if (age >= 18)
            {
                adultsCount++;
            }
        }

        double avgAge = (double)totalAge / _patients.Count;

        Console.WriteLine($"Всього: {_patients.Count}");
        Console.WriteLine($"Середній вік: {avgAge:F1} р.");
        Console.WriteLine($"Наймолодший: {_patients[minIdx].FullName} ({_patients[minIdx].Age} р.)");
        Console.WriteLine($"Найстарший: {_patients[maxIdx].FullName} ({_patients[maxIdx].Age} р.)");
        Console.WriteLine($"Дорослих: {adultsCount} з {_patients.Count}");
    }
}