using System;
using System.Collections.Generic;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T obj = new T();
        Type type = typeof(T);
        PropertyInfo[] props = type.GetProperties();

        for (int i = 0; i < props.Length; i++)
        {
            PropertyInfo prop = props[i];

            if (prop.GetSetMethod() == null) continue;

            while (true)
            {
                Console.Write(BuildPrompt(prop));
                string input = Console.ReadLine() ?? "";

                try
                {
                    object? converted = ConvertValue(input, prop.PropertyType);
                    prop.SetValue(obj, converted);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Некоректне значення, спробуйте ще раз.");
                }
            }
        }

        return obj;
    }

    private static string BuildPrompt(PropertyInfo prop)
    {
        List<string> hints = new List<string>();

        RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();
        if (required != null) hints.Add("обов'язкове");

        MaxLengthAttribute? max = prop.GetCustomAttribute<MaxLengthAttribute>();
        if (max != null) hints.Add($"макс. {max.Length}");

        MinValueAttribute? min = prop.GetCustomAttribute<MinValueAttribute>();
        if (min != null) hints.Add($"мін. {min.Min}");

        if (hints.Count == 0) return $"{prop.Name}: ";

        return $"{prop.Name} ({string.Join(", ", hints)}): ";
    }

    private static object? ConvertValue(string input, Type targetType)
    {
        Type actual = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (actual == typeof(string)) return input;

        if (actual.IsEnum) return Enum.Parse(actual, input, true);

        return Convert.ChangeType(input, actual);
    }
}
