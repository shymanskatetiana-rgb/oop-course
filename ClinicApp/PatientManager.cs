namespace ClinicApp;

public class  PatientManager
{
    private const int MaxPatients = 100;
    private Patient[] _patients = new Patient[MaxPatients];
    private int _count = 0;

    public int Count
    {
        get 
        { 
            return _count; 
        }
    }

    public void Add(Patient patient)
    {
        if (_count >= MaxPatients)
        {
            Console.WriteLine("Помилка: досягнуто ліміту пацієнтів (100).");
            return;
        }
        else
        {
            _patients[_count] = patient;
            _count++;
            Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
        }
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

    public Patient[] FindByName(string name)
    {
        string search = name.ToLower();
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(search) || _patients[i].LastName.ToLower().Contains(search))
            {
                matchCount++;
            }
        }

        Patient[] result = new Patient[matchCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(search) || _patients[i].LastName.ToLower().Contains(search))
            {
                result[index] = _patients[i];
                index++;
            }
        }

        return result;
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
        _patients[_count] = null;
        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_count} / {MaxPatients}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
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

        int totalAge = 0;
        int adultCount = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;

        for (int i = 0; i < _count; i++)
        {
            totalAge += _patients[i].Age;

            if (_patients[i].IsAdult)
            {
                adultCount++;
            }

            if (_patients[i].Age < _patients[youngestIndex].Age)
            {
                youngestIndex = i;
            }

            if (_patients[i].Age > _patients[oldestIndex].Age)
            {
                oldestIndex = i;
            }
        }

        double averageAge = (double)totalAge / _count;

        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:       {_count}");
        Console.WriteLine($"Середній вік: {averageAge:F1} р.");
        Console.WriteLine($"Наймолодший:  {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine($"Найстарший:   {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Дорослих:     {adultCount} з {_count}");
        Console.WriteLine("============================");
    }  
 }
