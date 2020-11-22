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

      
        MapController mapController = mapObject.AddComponent<MapController>();
        mapController.map = map;
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
                    fightController.map = map;
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
