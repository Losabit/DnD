using Assets.Scripts.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public GameObject mapObject;
    public GameObject playersObject;
    public GameObject gameController;
    public bool drawLines = true;

    public int[][] map = null;
    public Dictionary<int, Personnage.Initialization> personnagesInit;

    
    void Start()
    {
        if (map == null)
            map = MapGenerator.InitRandomMap(20, 20, 0.15f);
        
        DisplayMap(map, new Dictionary<int, string>
        {
            { 0, "Map/Floor" },
            { -1, "Map/Wall" }
        });
        DrawLines();
        mapObject.SetActive(true);

        List<Personnage> personnages = new List<Personnage>();
        if (personnagesInit != null)
        {
            foreach(int key in personnagesInit.Keys)
            {
                for(int i = 0; i < personnagesInit[key].numbers; i++)
                {
                    Models.Personnage personnageModel = PersonnageModding.GetPersonnage(personnagesInit[key].fichePath);
                    PlaceAtRandomPosition(personnageModel.Model, key);
                    personnages.Add(new Personnage(personnageModel, key));
                }
            }
            playersObject.SetActive(true);
        }

        FightController controller = gameController.AddComponent<FightController>();
        controller.map = map;
        controller.personnages = personnages;
        gameController.SetActive(true);
    }


    //void Update()
    //{
    //    /*
    //    if (gameObject.activeSelf &&
    //       (oldXSize != xSize || oldZSize != zSize || oldSpawnWallRate != spawnWallRate))
    //    {
    //        if (mapIsInit)
    //        {
    //            RemoveLines();
    //        }
    //        InitMap(xSize, zSize, spawnWallRate);
    //        mapIsInit = true;
    //        oldXSize = xSize;
    //        oldZSize = zSize;
    //        oldSpawnWallRate = spawnWallRate;
    //    }
    //    */
    //}


    // Map
    private void DisplayMap(int[][] map, Dictionary<int, string> mapping)
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
                transformObject.localPosition = vector;
                transformObject.SetParent(mapObject.transform, false);
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
        DrawLine(line, new Vector3(map[0].Length - decalageLine, 1, 0 - decalageLine), new Vector3(map[0].Length - decalageLine, 1, map[0].Length - decalageLine));
    }

    private void RemoveLines()
    {
        foreach (Transform child in mapObject.transform)
        {
            if(child.tag == "line")
                Destroy(child.gameObject);
        }
    }

    private void DrawLine(Material line, Vector3 start, Vector3 end)
    {
        GameObject myLine = new GameObject();
        myLine.transform.SetParent(mapObject.transform);
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();

        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = line;
        lr.startWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.tag = "line";
        // GameObject.Destroy(myLine, duration);
    }


    //Perosnnage
    private void PlaceAtRandomPosition(string path, int value)
    {
        Vector3 vector = new Vector3();

        do
        {
            vector.z = UnityEngine.Random.Range(0, map.Length);
            vector.x = UnityEngine.Random.Range(0, map[(int)vector.z].Length);
        } while (map[(int)vector.z][(int)vector.x] != 0);
        map[(int)vector.z][(int)vector.x] = 1;
        vector.y = 1;

        GameObject personnage = (GameObject)Resources.Load(path);
        if (personnage == null)
        {
            throw new Exception("can't load personnage : " + path);
        }
      
        Transform player = Instantiate(personnage.transform);
        player.position = vector;
        player.SetParent(playersObject.transform, false);
    }
}
