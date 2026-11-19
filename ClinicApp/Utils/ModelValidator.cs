using System;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        ValidationResult result = new ValidationResult();
        if (obj == null)
        {
            result.AddError("Object is null.");
            return result;
        }

        Type type = obj.GetType();
        PropertyInfo[] props = type.GetProperties();

        for (int i = 0; i < props.Length; i++)
        {
            PropertyInfo prop = props[i];
            if (!prop.CanRead) continue;

            object? value = prop.GetValue(obj);
            string name = prop.Name;

            RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();
            if (required != null)
            {
                bool emptyString = value is string str && string.IsNullOrWhiteSpace(str);
                if (value == null || emptyString)
                {
                    result.AddError($"{name}: {Fallback(required.ErrorMessage, "Field is required.")}");
                }
            }

            MaxLengthAttribute? max = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (max != null && value is string text && text.Length > max.Length)
            {
                result.AddError($"{name}: {Fallback(max.ErrorMessage, $"Must not exceed {max.Length} characters.")}");
            }

            MinValueAttribute? min = prop.GetCustomAttribute<MinValueAttribute>();
            if (min != null && value != null && IsNumeric(value))
            {
                double number = Convert.ToDouble(value);
                if (number < min.Min)
                {
                    result.AddError($"{name}: {Fallback(min.ErrorMessage, $"Must be at least {min.Min}.")}");
                }
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        Console.WriteLine(type.Name);

        PropertyInfo[] props = type.GetProperties();
        for (int i = 0; i < props.Length; i++)
        {
            PropertyInfo prop = props[i];
            Console.Write(prop.Name);

            Attribute[] attrs = Attribute.GetCustomAttributes(prop, true);
            for (int j = 0; j < attrs.Length; j++)
            {
                Type attrType = attrs[j].GetType();

                if (attrType.Namespace != typeof(RequiredAttribute).Namespace) continue;

                string attrName = attrType.Name;
                if (attrName.EndsWith("Attribute"))
                {
                    attrName = attrName.Substring(0, attrName.Length - "Attribute".Length);
                }

                Console.Write($" [{attrName}]");
            }

            Console.WriteLine();
        }
    }

    private static bool IsNumeric(object value)
    {
        TypeCode code = Type.GetTypeCode(value.GetType());
        return code == TypeCode.Byte || code == TypeCode.SByte
            || code == TypeCode.Int16 || code == TypeCode.UInt16
            || code == TypeCode.Int32 || code == TypeCode.UInt32
            || code == TypeCode.Int64 || code == TypeCode.UInt64
            || code == TypeCode.Single || code == TypeCode.Double
            || code == TypeCode.Decimal;
    }

    private static string Fallback(string message, string fallback)
    {
        return string.IsNullOrWhiteSpace(message) ? fallback : message;
    }
}