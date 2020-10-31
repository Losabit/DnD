using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PersonnageEditor : MonoBehaviour
{
    public GameObject playersGameObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ImportOnClick()
    {
        //  string path = EditorUtility.OpenFilePanel("Load Model", "", "");
        /*
        File.Create(Application.dataPath + "/Resources/Personnages/test.json");
        string path = null;
        if (path != null)
        {
            Debug.Log(path);
            File.Copy(path, "Assets/Resources/Personnages/" + Path.GetFileName(path));

            //Transform child = playersGameObject.transform.GetChild(0);
            //child.GetComponent<Player>().path = "Personnages/" + Path.GetFileNameWithoutExtension(path);

            playersGameObject.GetComponents<Player>()[0].path = "Personnages/" + Path.GetFileNameWithoutExtension(path);
        }
        */
    }

    public void PersonnagesOnClick(GameObject toActivate)
    {
        toActivate.SetActive(!toActivate.activeSelf);
    }
}
