using System;
using System.Collections.Generic;
using UnityEngine;

class MapController : MonoBehaviour
{
    public int xSize = 10;
    public int zSize = 10;
    public int[][] map = null;
    public string loadMap = string.Empty;

    private int oldXSize = 10;
    private int oldZSize = 10;

    private void Awake()
    {
        if (map != null)
        {
            DisplayMap(map, new Dictionary<int, string>
            {
                { 0, "Map/Objects/Floor" },
                { -1, "Map/Objects/Wall" }
            });
            zSize = oldZSize = map.Length;
            xSize = oldXSize = map[0].Length;
            DrawLines();
        }
    }

    private void Update()
    {
        if (gameObject.activeSelf &&
           (oldXSize != xSize || oldZSize != zSize))
        {
            RemoveLines();

            int[][] newMap = new int[zSize][];
            for (int z = 0; z < zSize; z++)
            {
                newMap[z] = new int[xSize];
                for (int x = 0; x < xSize; x++)
                {
                    if (z < oldZSize && x < oldXSize)
                    {
                        newMap[z][x] = map[z][x];
                    }
                    else if (xSize > oldXSize || zSize > oldZSize)
                    {
                        newMap[z][x] = 0;
                        GameObject prefab = (GameObject)Resources.Load("Map/Objects/Floor");
                        Transform transformObject = Instantiate(prefab.transform);
                        MapModding.ItemStorage storage = transformObject.gameObject.AddComponent<MapModding.ItemStorage>();
                        storage.obj = "Map/Objects/Floor";
                        storage.z = z;
                        storage.x = x;
                        transformObject.localPosition = new Vector3(x, 0, z);
                        transformObject.SetParent(transform, false);
                    }
                }
            }

            if (xSize < oldXSize || zSize < oldZSize)
            {
                foreach(Transform child in transform)
                {   
                    if (child.position.x >= xSize && child.position.x < oldXSize
                        || child.position.z >= zSize && child.position.z < oldZSize)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }

            map = newMap;
            oldXSize = xSize;
            oldZSize = zSize;
            DrawLines();
        }

        if(loadMap != string.Empty)
        {
            Models.Map mapToLoad = MapModding.LoadMap(loadMap);
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            map = mapToLoad.CasesModels;
            zSize = oldZSize = map.Length;
            xSize = oldXSize = map[0].Length;
            DisplayMap(mapToLoad);
            DrawLines();
            loadMap = string.Empty;
        }
    }

    private void DisplayMap(Models.Map mapToLoad)
    {
        DisplayMap(mapToLoad.CasesModels, MapModding.RessourcesToDict(mapToLoad.Models),
            mapToLoad.CasesTextures, MapModding.RessourcesToDict(mapToLoad.Textures));
    }

    private void DisplayMap(int[][] map, Dictionary<int, string> mapping,
        int[][] textureMap = null, Dictionary<int, string> textureMapping = null)
    {
        Vector3 vector = new Vector3();
        Dictionary<int, GameObject> objectMapping = MapGenerator.GetAssociateGameObject(mapping);

        for (int z = 0; z < map.Length; z++)
        {
            vector.z = z;
            for (int x = 0; x < map[z].Length; x++)
            {
                vector.x = x;

                if (!mapping.ContainsKey(map[z][x]))
                {
                    throw new Exception("Mapping missing a key : " + map[z][x]);
                }
                Transform transformObject = Instantiate(objectMapping[map[z][x]].transform);
                MapModding.ItemStorage storage = transformObject.gameObject.AddComponent<MapModding.ItemStorage>();
                storage.obj = mapping[map[z][x]];
                storage.z = z;
                storage.x = x;

                if(textureMap != null && textureMapping != null
                    && textureMap[z][x] != 0 && textureMapping.ContainsKey(textureMap[z][x]))
                {
                    // enlever le jpg car il peut poser probleme
                    string path = "C:/Users/quent/Documents/Unity/DnD/Assets/Resources/" + textureMapping[textureMap[z][x]] + ".jpg";
                    transformObject.GetComponent<MeshRenderer>().material.mainTexture = IMG2Sprite.LoadTexture(path);
                    storage.texture = textureMapping[textureMap[z][x]];
                }
                transformObject.localPosition = vector;
                transformObject.SetParent(transform, false);
            }
        }
    }

    private void DrawLines()
    {
        Material line = (Material)Resources.Load("Map/LineMaterial");
        float decalageLine = 0.5f;

        for (int z = 0; z < map.Length; z++)
        {
            for (int x = 0; x < map[z].Length; x++)
            {
                DrawLine(line, new Vector3(x - decalageLine, 1, 0 - decalageLine), new Vector3(x - decalageLine, 1, map.Length - decalageLine));
            }

            DrawLine(line, new Vector3(0 - decalageLine, 1, z - decalageLine), new Vector3(map[z].Length - decalageLine, 1, z - decalageLine));
        }

        DrawLine(line, new Vector3(0 - decalageLine, 1, map.Length - decalageLine), new Vector3(map[0].Length - decalageLine, 1, map.Length - decalageLine));
       //DrawLine(line, new Vector3(map[0].Length - decalageLine, 1, 0 - decalageLine), new Vector3(map[0].Length - decalageLine, 1, map[0].Length - decalageLine));
    }

    private void RemoveLines()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "line")
                Destroy(child.gameObject);
        }
    }

    private void DrawLine(Material line, Vector3 start, Vector3 end)
    {
        GameObject myLine = new GameObject();
        myLine.transform.SetParent(transform);
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();

        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = line;
        lr.startWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.tag = "line";
    }
}

