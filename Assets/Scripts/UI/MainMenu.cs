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
        gameLoader.SetActive(true);
        gameObject.SetActive(false);
    }

    public void EditorButtonOnClick(GameObject toActivate)
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
        gameLoader.SetActive(true);
        gameObject.SetActive(false);
    }
}

