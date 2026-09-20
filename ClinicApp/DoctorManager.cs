namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50;
    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count = 0;

    public int Count
    {
        get
        {
            return _count;
        }
    }

    public void Add(Doctor doctor)
    {
        if (_count >= MaxDoctors)
        {
            Console.WriteLine("Помилка: досягнуто ліміту лікарів (50).");
            return;
        }

        _doctors[_count] = doctor;
        _count++;
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }
        return null;
    }

    public Doctor[] FindBySpeciality(string speciality)
    {
        string search = speciality.ToLower();
        int matchCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(search))
            {
                matchCount++;
            }
        }

        Doctor[] result = new Doctor[matchCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToLower().Contains(search))
            {
                result[index] = _doctors[i];
                index++;
            }
        }

        return result;
    }

    public Doctor[] GetAll()
    {
        Doctor[] copy = new Doctor[_count];
        for (int i = 0; i < _count; i++)
        {
            copy[i] = _doctors[i];
        }
        return copy;
    }

    public bool Remove(int id)
    {
        int targetIndex = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
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
            _doctors[i] = _doctors[i + 1];
        }

        _count--;
        _patientsNull();
        return true;
    }

    private void _patientsNull()
    {
        _doctors[_count] = null!;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        Console.WriteLine($"=== Лікарі ({_count} / {MaxDoctors}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i]);
        }
        Console.WriteLine("____________________________________________________________");
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("Статистика недоступна: список порожній.");
            return;
        }

        int availableNowCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].IsAvailableNow)
            {
                availableNowCount++;
            }
        }

        Console.WriteLine("=== Статистика лікарів ===");
        Console.WriteLine($"Всього:         {_count}");
        Console.WriteLine($"Доступні зараз: {availableNowCount}");
        Console.WriteLine("По спеціальностях:");

        for (int i = 0; i < _count; i++)
        {
            bool isFirst = true;
            for (int j = 0; j < i; j++)
            {
                if (string.Equals(_doctors[i].Speciality, _doctors[j].Speciality, StringComparison.OrdinalIgnoreCase))
                {
                    isFirst = false;
                    break;
                }
            }

            if (isFirst)
            {
                int specCount = 0;
                for (int k = 0; k < _count; k++)
                {
                    if (string.Equals(_doctors[k].Speciality, _doctors[i].Speciality, StringComparison.OrdinalIgnoreCase))
                    {
                        specCount++;
                    }
                }
                Console.WriteLine($"  {_doctors[i].Speciality}: {specCount}");
            }
        }

        Console.WriteLine("==========================");
    }
}