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
    public GameObject viewObject;
    public bool drawLines = true;
    public ControllerType controllerType = ControllerType.None;

    public int[][] map = null;
    public Dictionary<int, Personnage.Initialization> personnagesInit;


    void Start()
    {
        if (map == null)
            throw new Exception("map not given");

        DisplayMap(map, new Dictionary<int, string>
        {
            { 0, "Map/Floor" },
            { -1, "Map/Wall" }
        });
        DrawLines();
        mapObject.SetActive(true);

        List<Personnage> personnages = new List<Personnage>();
        List<FightPlayController> personnagesController = new List<FightPlayController>();
        if (personnagesInit != null)
        {
            foreach (int key in personnagesInit.Keys)
            {
                for (int i = 0; i < personnagesInit[key].numbers; i++)
                {
                    Models.Personnage personnageModel = PersonnageModding.GetPersonnage(personnagesInit[key].fichePath);
                    Personnage fichePersonnage = new Personnage(personnageModel, key);
                    FightPlayController fightController = PlaceAtRandomPosition(personnageModel.Model, key);
                    fightController.personnage = fichePersonnage;
                    personnagesController.Add(fightController);
                    personnages.Add(fichePersonnage);
                }

            }
            playersObject.SetActive(true);
        }

        if (controllerType != ControllerType.None)
        {
            LoadCorrectController(personnages, personnagesController);
        }


    }

    private void LoadCorrectController(List<Personnage> personnages, List<FightPlayController> personnagesController)
    {

        if (controllerType == ControllerType.Fight)
        {
            FightController controller = gameController.AddComponent<FightController>();
            controller.map = map;
            controller.personnages = personnages;
            controller.fightPlayController = personnagesController;
            controller.view = viewObject;
            gameController.SetActive(true);
        }
        else if (controllerType == ControllerType.Editor)
        {
            EditorController controller = gameController.AddComponent<EditorController>();
            controller.map = map;
            controller.personnages = personnages;
            gameController.SetActive(true);
        }
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
            if (child.tag == "line")
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


    //Personnage
    private FightPlayController PlaceAtRandomPosition(string path, int value)
    {
        Vector3 vector = new Vector3();

        do
        {
            vector.z = UnityEngine.Random.Range(0, map.Length);
            vector.x = UnityEngine.Random.Range(0, map[(int)vector.z].Length);
        } while (map[(int)vector.z][(int)vector.x] != 0);
        map[(int)vector.z][(int)vector.x] = value;
        vector.y = 1;

        GameObject personnage = (GameObject)Resources.Load(path);
        if (personnage == null)
        {
            throw new Exception("can't load personnage : " + path);
        }

        Transform player = Instantiate(personnage.transform);
        player.position = vector;
        player.SetParent(playersObject.transform, false);
        if(value == 1)
        {
            return player.gameObject.AddComponent<PlayerController>();
        }
        else
        {
            return player.gameObject.AddComponent<IAController>();
        }
       
    }
}
