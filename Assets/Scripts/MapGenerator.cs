using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static int[][] InitRandomMap(int xSize, int zSize, float spawnWallRate)
    {
        int[][] map = new int[zSize][];
        
        for (int z = 0; z < map.Length; z++)
        {
            map[z] = new int[xSize];
            for (int x = 0; x < map[z].Length; x++)
            {
                if (UnityEngine.Random.Range(0f, 1f) > spawnWallRate)
                {
                   
                    map[z][x] = 0;
                }
                else
                {
                    map[z][x] = -1;
                }
            }
        }
        return map;
    }

    public static Dictionary<int, GameObject> GetAssociateGameObject(Dictionary<int, string> mapping)
    {
        Dictionary<int, GameObject> result = new Dictionary<int, GameObject>();
        foreach (var key in mapping.Keys)
        {
            GameObject prefab = (GameObject)Resources.Load(mapping[key]);
            if (prefab == null)
            {
                throw new Exception("File not found for Map Generation");
            }
            result.Add(key, prefab);
        }
        return result;
    }

    
}