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

    public Diagnosis[] GetDiagnoses(int patientId)
    {
        int matching = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis)
            {
                matching++;
            }
        }

        Diagnosis[] result = new Diagnosis[matching];

        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis diagnosis)
            {
                result[index] = diagnosis;
                index++;
            }
        }

        return result;

    }

    public LabResult[] GetLabResults(int patientId)
    {
        int matching = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult)
            {
                matching++;
            }
        }

        LabResult[] result = new LabResult[matching];

        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult labResult)
            {
                result[index] = labResult;
                index++;
            }
        }

        return result;
    }

    public Prescription[] GetPrescriptions(int patientId)
    {
        int matching = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription)
            {
                matching++;
            }
        }

        Prescription[] result = new Prescription[matching];

        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription prescription)
            {
                result[index] = prescription;
                index++;
            }
        }

        return result;
    }

    public Diagnosis[] GetChronicDiagnoses(int patientId)
    {
        int matching = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic)
            {
                matching++;
            }
        }

        Diagnosis[] result = new Diagnosis[matching];

        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId
                && _records[i] is Diagnosis d
                && d.IsChronic)
            {
                result[index] = d;
                index++;
            }
        }

        return result;
    }

    public Prescription[] GetActivePrescriptions(int patientId)
    {
        int matching = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive())
            {
                matching++;
            }
        }

        Prescription[] result = new Prescription[matching];

        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId
                && _records[i] is Prescription p
                && p.IsActive())
            {
                result[index] = p;
                index++;
            }
        }

        return result;
    }

    public void DisplayPatientSummary(int patientId)
    {
        MedicalRecord[] records = GetByPatient(patientId);

        if (records.Length == 0)
        {
            Console.WriteLine($"У пацієнта #{patientId} немає медичних записів.");
            return;
        }

        int diagnosesCount = 0;
        int labResultsCount = 0;
        int prescriptionsCount = 0;

        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Diagnosis)
                diagnosesCount++;
            else if (records[i] is LabResult)
                labResultsCount++;
            else if (records[i] is Prescription)
                prescriptionsCount++;
        }

        Console.WriteLine($"=== Медична картка пацієнта #{patientId} ===");
        Console.WriteLine($"Всього записів: {records.Length} (діагнозів: {diagnosesCount}, аналізів: {labResultsCount}, рецептів: {prescriptionsCount})");

        Diagnosis[] chronic = GetChronicDiagnoses(patientId);
        if (chronic.Length > 0)
        {
            Console.WriteLine($"\nХронічні діагнози ({chronic.Length}):");
            for (int i = 0; i < chronic.Length; i++)
            {
                Console.WriteLine(chronic[i]);
            }
        }

        Prescription[] active = GetActivePrescriptions(patientId);
        if (active.Length > 0)
        {
            Console.WriteLine($"\nАктивні рецепти ({active.Length}):");
            for (int i = 0; i < active.Length; i++)
            {
                Console.WriteLine($"{active[i]} — до {active[i].ExpiresAt:dd.MM.yyyy}");
            }
        }
    }
}
