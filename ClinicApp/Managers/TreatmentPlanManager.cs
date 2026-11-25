using System;
using System.Collections.Generic;
using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Utils;
using ClinicApp.Events;

namespace ClinicApp.Managers;

public class TreatmentPlanManager
{
    private readonly List<TreatmentPlan> _plans = new List<TreatmentPlan>();

    public int Count => _plans.Count;

    public event EventHandler<TreatmentPlanEventArgs>? PlanCompleted;

    public bool Complete(int planId)
    {
        TreatmentPlan? plan = GetById(planId);
        if (plan != null && plan.Complete())
        {
            PlanCompleted?.Invoke(this, new TreatmentPlanEventArgs(plan.Id, plan.PatientId, plan.Diagnosis));
            return true;
        }
        return false;
    }

    public bool Add(TreatmentPlan plan)
    {
        ValidationResult result = ModelValidator.Validate(plan);

        if (!result.IsValid)
        {
            Console.WriteLine("ѕлан не додано Ч знайдено помилки:");
            result.Print();
            return false;
        }

        _plans.Add(plan);
        return true;
    }

    public TreatmentPlan? GetById(int id)
    {
        for (int i = 0; i < _plans.Count; i++)
        {
            if (_plans[i].Id == id) return _plans[i];
        }
        return null;
    }

    public TreatmentPlan[] GetByPatient(int patientId)
    {
        List<TreatmentPlan> found = new List<TreatmentPlan>();
        for (int i = 0; i < _plans.Count; i++)
        {
            if (_plans[i].PatientId == patientId) found.Add(_plans[i]);
        }
        return found.ToArray();
    }

    public TreatmentPlan[] GetByStatus(TreatmentStatus status)
    {
        List<TreatmentPlan> found = new List<TreatmentPlan>();
        for (int i = 0; i < _plans.Count; i++)
        {
            if (_plans[i].Status == status) found.Add(_plans[i]);
        }
        return found.ToArray();
    }

    public TreatmentPlan[] GetAll()
    {
        return _plans.ToArray();
    }

    public void DisplayList(TreatmentPlan[] plans)
    {
        if (plans.Length == 0)
        {
            Console.WriteLine("ѕлан≥в не знайдено.");
            return;
        }

        for (int i = 0; i < plans.Length; i++)
        {
            Console.WriteLine(plans[i]);
        }
    }

    public void DisplayAll()
    {
        DisplayList(GetAll());
    }
}
