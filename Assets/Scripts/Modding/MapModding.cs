using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;

public class MapModding
{
    public enum ObjectType
    {
        Floor,
        Hole,
        Wall,
        Door
    }

    public class ItemStorage : MonoBehaviour
    {
        public string texture = string.Empty;
        public string obj = string.Empty;
        public int x = -1;
        public int z = -1;
    }

    public static Models.Map LoadMap(string path)
    {
        string content = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<Models.Map>(content);
    }

    public static void SaveMap(Models.Map mapModel, string filename)
    {
        string jsonContent = JsonConvert.SerializeObject(mapModel);
        string path = "C:/Users/quent/Documents/Unity/DnD/Assets/Resources/Map/SavedMaps/" + filename + ".json";
        File.Create(path).Close();
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(jsonContent);
        }
    }

    public static void SaveMap(GameObject obj, string filename)
    {
        MapController mapController = obj.GetComponent<MapController>();
        Models.Map mapModel = Models.Map.Instancied(mapController.xSize, mapController.zSize);

        for(int i = 0; i < obj.transform.childCount; i++)
        {
            Transform child = obj.transform.GetChild(i);
            if (child.tag == "line")
                continue;

            ItemStorage storage = child.GetComponent<ItemStorage>();
            if (storage == null)
                throw new System.Exception("ItemStorage not found");

            List<Models.ExternalRessource> filteredModels = mapModel.Models.Where(x => x.Path == storage.obj).ToList();
            mapModel.CasesModels[storage.z][storage.x] = GetCorrectRessourceValue(mapModel.Models, storage.obj);
            if (filteredModels.Count() == 0)
            {
                Models.ExternalRessource objModel = new Models.ExternalRessource();
                objModel.Path = storage.obj;
                objModel.Value = -mapModel.Models.Count - 1;
                mapModel.Models.Add(objModel);
            }
           
            List<Models.ExternalRessource> filteredTextures = mapModel.Textures.Where(x => x.Path == storage.texture).ToList();
            mapModel.CasesTextures[storage.z][storage.x] = GetCorrectRessourceValue(mapModel.Textures, storage.texture);
            if (filteredTextures.Count() == 0)
            {
                Models.ExternalRessource textureModel = new Models.ExternalRessource();
                textureModel.Path = storage.texture;
                textureModel.Value = mapModel.CasesTextures[storage.z][storage.x];
                mapModel.Textures.Add(textureModel);
            }
        }
        SaveMap(mapModel, filename);
    }

    private static int GetCorrectRessourceValue(List<Models.ExternalRessource> ressources, string path)
    {
        List<Models.ExternalRessource> filteredRessources = ressources.Where(x => x.Path == path).ToList();
        if (path == string.Empty)
            return 0;
        else if(filteredRessources.Count() == 0)
            return -ressources.Count - 1;
        else
            return filteredRessources[0].Value;
    }

    public static Dictionary<int, string> RessourcesToDict(List<Models.ExternalRessource> externalRessources)
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        foreach(var ressource in externalRessources)
        {
            result.Add(ressource.Value, ressource.Path);
        }
        return result;
    }
}
