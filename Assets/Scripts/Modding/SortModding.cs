using Models;
using System;
using UnityEngine;
using System.Collections.Generic;

public class SortModding
{
    public static List<Vector3> GetTouchedCases(Sorts sort, Vector3 personnagePosition)
    {
        List<Vector3> casesCanBeTouched = new List<Vector3>();
        for (int i = -sort.MaximumScope; i <= sort.MaximumScope; i++)
        {
            for (int j = -sort.MaximumScope; j <= sort.MaximumScope; j++)
            {
                Vector3 vector = new Vector3(personnagePosition.x + i, personnagePosition.y, personnagePosition.z + j);
                int distance = (int)CaseDistance(vector, personnagePosition);
                if (distance >= sort.MinimumScope && distance <= sort.MaximumScope)
                {
                    casesCanBeTouched.Add(vector);
                }
            }
        }
        return casesCanBeTouched;
    }

    public static List<Vector3> GetAttackCases(Vector3 hit, Sorts sort)
    {
        List<Vector3> vectors = new List<Vector3>();
        for (int i = 0; i < sort.Cases.Length; i++)
        {
            vectors.Add(new Vector3(hit.x + sort.Cases[i][0], hit.y, hit.z + sort.Cases[i][1]));
        }
        return vectors;
    }

    private static float CaseDistance(Vector3 start, Vector3 end)
    {
        return Mathf.Abs(start.x - end.x) +
            Mathf.Abs(start.y - end.y) +
            Mathf.Abs(start.z - end.z);
    }
}

