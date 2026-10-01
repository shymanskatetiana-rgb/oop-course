namespace ClinicApp;

public class GrowablePatientManager
{
    private Patient[] _patients = new Patient[4];
    private int _count = 0;

    public int Count
    {
        get
        {
            return _count;
        }
    }

    public int Capacity
    {
        get
        {
            return _patients.Length;
        }
    }

    private void Grow()
    {
        int oldCapacity = _patients.Length;
        int newCapacity = oldCapacity * 2;
        Console.WriteLine($"  Масив заповнений! Розширення: {oldCapacity} \u2192 {newCapacity}");

        Patient[] newArray = new Patient[newCapacity];
        for (int i = 0; i < _count; i++)
        {
            newArray[i] = _patients[i];
        }

        _patients = newArray;
    }

    public void Add(Patient patient)
    {
        if (_count == _patients.Length)
        {
            Grow();
        }

        _patients[_count] = patient;
        _count++;
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public bool Remove(int id)
    {
        int targetIndex = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex == -1)
        {
            return false;
        }

        for (int i = targetIndex; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }

        _count--;
        _patients[_count] = null!;
        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_count} / {Capacity}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
    }
}