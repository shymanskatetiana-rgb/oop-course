using System;
using System.Collections.Generic;
using ClinicApp.Interfaces;

namespace ClinicApp.Managers;

public class Repository<T> where T : IIdentifiable
{
    private List<T> _items = new List<T>();

    public int Count
    {
        get { return _items.Count; }
    }

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T GetById(int id)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Id == id)
            {
                return _items[i];
            }
        }
        return default!;
    }

    public T[] GetAll()
    {
        return _items.ToArray();
    }

    public bool Remove(int id)
    {
        T item = GetById(id);
        if (item != null)
        {
            _items.Remove(item);
            return true;
        }
        return false;
    }
}