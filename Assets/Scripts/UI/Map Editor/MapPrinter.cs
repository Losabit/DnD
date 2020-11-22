using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapPrinter : MonoBehaviour
{
    public Text text;
    public GameObject map;
    public string usedFile = string.Empty;

    private void Start()
    {
        string[] mapsPath = Directory.GetFiles("C:/Users/quent/Documents/Unity/DnD/Assets/Resources/Map/SavedMaps");
        foreach (string path in mapsPath)
        {
            if (!path.EndsWith(".json"))
                continue;
            string filename = Path.GetFileName(path);

            text.text = filename.Remove(filename.LastIndexOf(".")); ;
            Transform transformObject = Instantiate(text.transform);
            MapSelector textureSelector = transformObject.gameObject.AddComponent<MapSelector>();
            textureSelector.filename = filename;
            textureSelector.outerReference = this;
            transformObject.SetParent(transform, false);
        }
    }

    private void Update()
    {
        if(usedFile != string.Empty)
        {
            map.GetComponent<MapController>().loadMap = "C:/Users/quent/Documents/Unity/DnD/Assets/Resources/Map/SavedMaps/" + usedFile;
            usedFile = string.Empty;
        }
    }

    class MapSelector : MonoBehaviour, IPointerClickHandler
    {
        public string filename;
        public MapPrinter outerReference;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                outerReference.usedFile = filename;
            }
        }
    }
}

