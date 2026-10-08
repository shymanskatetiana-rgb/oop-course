using System;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            throw new ArgumentException("Ім'я не може бути порожнім або довшим за 50 символів.", fieldName);
    }

    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone) || phone.Length != 10 || !long.TryParse(phone, out _))
            throw new ArgumentException("Номер телефону має складатися з 10 цифр.");
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today || value.Year < 1900)
            throw new ArgumentOutOfRangeException(fieldName, "Дата народження не може бути в майбутньому або раніше 1900 року.");
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(fieldName, "Значення повинно бути більше 0.");
    }
}