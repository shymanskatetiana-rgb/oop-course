using System;
using System.IO;
using System.Text;

namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    public void LogInfo(string message)
    {
        Write("INFO", message);
    }

    public void LogWarning(string message)
    {
        Write("WARN", message);
    }

    public void LogError(string message)
    {
        Write("ERROR", message);
    }

    public string[] GetLastLines(int n)
    {
        if (!File.Exists(_logPath) || n <= 0)
        {
            return Array.Empty<string>();
        }

        string[] all = File.ReadAllLines(_logPath, Encoding.UTF8);
        if (n >= all.Length)
        {
            return all;
        }

        string[] result = new string[n];
        int start = all.Length - n;
        for (int i = 0; i < n; i++)
        {
            result[i] = all[start + i];
        }
        return result;
    }

    public void Clear()
    {
        using StreamWriter writer = new StreamWriter(_logPath, false, Encoding.UTF8);
    }

    public bool Exists()
    {
        return File.Exists(_logPath);
    }

    private void Write(string level, string message)
    {
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level,-5}] {message}" + Environment.NewLine;
        File.AppendAllText(_logPath, line, Encoding.UTF8);
    }
}