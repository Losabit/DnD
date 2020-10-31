using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PersonnageModding
{
    public PersonnageModding()
    {
        
    }

    public static List<string> GetAllPersonnagesName()
    {
        return Directory.GetFiles("Assets/Resources/Personnages")
            .Where(x => x.EndsWith(".json"))
            .Select(x => Path.GetFileName(x))
            .Select(x => x.Replace(".json", "")).ToList();
    }

    public static Models.Personnage GetPersonnage(string fileName)
    {
        if (!File.Exists(Application.dataPath + "/Resources/Personnages/" + fileName + ".json"))
            throw new System.Exception("can't find file : " + Application.dataPath + "/Resources/Personnages/" + fileName + ".json");

        //var jsonTextFile = Resources.Load<TextAsset>("Personnages/" + fileName);
        //Models.Personnage personnage = JsonUtility.FromJson<Models.Personnage>(jsonTextFile.text);
        
        string fileContent = File.ReadAllText(Application.dataPath + "/Resources/Personnages/" + fileName + ".json");
        Models.PersonnageModel personnage = JsonConvert.DeserializeObject<Models.PersonnageModel>(fileContent);
        return personnage.Personnage;
    }
}
