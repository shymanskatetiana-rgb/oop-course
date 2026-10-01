namespace ClinicApp;

public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bt) => bt switch
    {
        BloodType.APositive => "A+",
        BloodType.ANegative => "A-",
        BloodType.BPositive => "B+",
        BloodType.BNegative => "B-",
        BloodType.ABPositive => "AB+",
        BloodType.ABNegative => "AB-",
        BloodType.OPositive => "O+",
        BloodType.ONegative => "O-",
        _ => "Невідомо"
    };

    public static string FormatSpeciality(Speciality s) => s switch
    {
        Speciality.General => "Загальна практика",
        Speciality.Cardiology => "Кардіологія",
        Speciality.Neurology => "Неврологія",
        Speciality.Pediatrics => "Педіатрія",
        Speciality.Surgery => "Хірургія",
        Speciality.Orthopedics => "Ортопедія",
        Speciality.Dermatology => "Дерматологія",
        Speciality.Emergency => "Невідкладна допомога",
        _ => s.ToString()
    };

    public static string FormatAge(int age)
    {
        int rem100 = age % 100;
        int rem10 = age % 10;

        if (rem100 >= 11 && rem100 <= 19)
        {
            return $"{age} років";
        }
        else if (rem10 == 1)
        {
            return $"{age} рік";
        }
        else if (rem10 >= 2 && rem10 <= 4)
        {
            return $"{age} роки";
        }
        else
        {
            return $"{age} років";
        }
    }

    public static string FormatPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return phone;
        }

        if (phone.Length == 10)
        {
            bool allDigits = true;
            foreach (char c in phone)
            {
                if (!char.IsDigit(c))
                {
                    allDigits = false;
                    break;
                }
            }

            if (allDigits)
            {
                return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6, 4)}";
            }
        }

        return phone;
    }
}