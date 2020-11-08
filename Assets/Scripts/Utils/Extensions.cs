using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Extensions
{
    public static List<T> Clone<T>(this List<T> listToClone)
    {
        List<T> result = new List<T>();
        foreach(T value in listToClone)
        {
            result.Add(value);
        }
        return result;
    }
}