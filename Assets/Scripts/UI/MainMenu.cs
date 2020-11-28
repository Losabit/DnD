using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameLoader;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {


        }
    }

    public void PlayButtonOnClick(GameObject toActivate)
    {
        toActivate.SetActive(true);
        Dictionary<int, Personnage.Initialization> dict = new Dictionary<int, Personnage.Initialization>();
        dict.Add(1, new Personnage.Initialization
        {
            fichePath = "eveille",
            numbers = 1
        });
        dict.Add(2, new Personnage.Initialization
        {
            fichePath = "orc",
            numbers = 3
        });
        GameLoader loader = gameLoader.GetComponent<GameLoader>();
        loader.personnagesInit = dict;
        loader.map = MapGenerator.InitRandomMap(20, 20, 0.15f);
        loader.viewObject = toActivate;
        loader.controllerType = Assets.Scripts.Controllers.ControllerType.Fight;
        gameLoader.SetActive(true);
        gameObject.SetActive(false);
    }

    public void EditorButtonOnClick(GameObject toActivate)
    {
        toActivate.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PersonnageEditorButtonOnClick(GameObject toActivate)
    {
        toActivate.SetActive(true);
        
        Dictionary<int, Personnage.Initialization> dict = new Dictionary<int, Personnage.Initialization>();
        dict.Add(1, new Personnage.Initialization
        {
            fichePath = "eveille",
            numbers = 1
        });
        GameLoader loader = gameLoader.GetComponent<GameLoader>();
        loader.personnagesInit = dict;
        loader.map = MapGenerator.InitRandomMap(20, 20, 0f);
        loader.controllerType = Assets.Scripts.Controllers.ControllerType.Editor;
        gameLoader.SetActive(true);
        gameObject.SetActive(false);
    }

    public void MapEditorButtonOnClick(GameObject toActivate)
    {
        toActivate.SetActive(true);
        Camera.main.gameObject.AddComponent<MapExplorer>();
        GameLoader loader = gameLoader.GetComponent<GameLoader>();
        loader.map = MapGenerator.InitRandomMap(20, 20, 0f);
        loader.controllerType = Assets.Scripts.Controllers.ControllerType.Editor;
        gameLoader.SetActive(true);
        gameObject.SetActive(false);
    }
}

