using System;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private const int Limit = 1000;
    private MedicalRecord[] _records = new MedicalRecord[Limit];
    private int _count;

    public int Count
    {
        get { return _count; }
    }

    public void Add(MedicalRecord record)
    {
        if (_count >= Limit)
        {
            Console.WriteLine($"Помилка: досягнуто ліміт ({Limit}) записів.");
            return;
        }

        _records[_count] = record;
        _count++;

        Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
                return _records[i];
        }
        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int matching = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
                matching++;
        }

        MedicalRecord[] result = new MedicalRecord[matching];

        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                result[index] = _records[i];
                index++;
            }
        }

        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int matching = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
                matching++;
        }

        MedicalRecord[] result = new MedicalRecord[matching];

        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                result[index] = _records[i];
                index++;
            }
        }

        return result;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Записів немає.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_records[i]);
        }
    }

    public void DisplayList(MedicalRecord[] records)
    {
        if (records.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }

        for (int i = 0; i < records.Length; i++)
        {
            Console.WriteLine(records[i]);
        }
    }

    public MedicalRecord? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                return null;
            return _records[index];
        }
    }
}
