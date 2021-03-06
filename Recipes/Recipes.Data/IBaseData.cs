﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Recipes.Data
{
    public interface IBaseData<T>
    {
        IEnumerable<T> GetAll();
        T GetById(string Id);
        List<T> GetBy(string fieldName, string value);
        T Update(T item);
        T Add(T item);
        T Delete(string Id);
    }
}
