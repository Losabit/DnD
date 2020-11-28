using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class DungeonPrinter : MonoBehaviour
{
    public GameObject dungeonDetail;
    public Text text;
    public string usedPath = string.Empty;
    public GameObject dungeonEditor;

    private bool isActive = false;
    private Text historyText;
    private Image image;
    private Text titleText;
    private Button editButton;

    private void Start()
    {
        text.fontSize = 10;
        text.alignment = TextAnchor.UpperLeft;
        text.text = "Create new";
        Instanciate(text, "None");

        string path = "C:/Users/quent/Documents/Unity/DnD/Assets/Resources/Dungeon";
        string[] directories = Directory.GetDirectories(path);
        foreach (string directory in directories)
        {
            text.text = directory.Replace(path, "").Remove(0, 1);
            Instanciate(text, directory);
        }

        if (dungeonDetail != null)
        {
            foreach (Transform child in dungeonDetail.transform)
            {
                switch(child.name)
                {
                    case "Title":
                        titleText = child.GetComponent<Text>();
                        break;
                    case "Image":
                        image = child.GetComponent<Image>();
                        break;
                    case "History":
                        historyText = child.GetComponent<Text>();
                        break;
                    case "EditButton":
                        editButton = child.GetComponent<Button>();
                        break;
                }
            }
        }
    }

    private void Instanciate(Text textToInstancied, string path)
    {
        Transform transformObject = Instantiate(textToInstancied.transform);
        DungeonSelector dungeonSelector = transformObject.gameObject.AddComponent<DungeonSelector>();
        dungeonSelector.path = path;
        dungeonSelector.outerReference = this;
        transformObject.SetParent(transform, false);
    }

    private void ActiveAllComponents(bool active)
    {
        titleText.gameObject.SetActive(active);
        historyText.gameObject.SetActive(active);
        image.gameObject.SetActive(active);
        editButton.gameObject.SetActive(active);
        isActive = active;
    }

    private void Update()
    {
        if (usedPath != string.Empty)
        {
            if (usedPath == "None")
            {
                if(isActive)
                    ActiveAllComponents(false);
                editButton.gameObject.SetActive(true);
                editButton.GetComponentInChildren<Text>().text = "Create";
                editButton.onClick.AddListener(delegate {
                    gameObject.transform.parent.parent.gameObject.SetActive(false);
                    dungeonEditor.SetActive(true);
                });
                usedPath = string.Empty;
            }
            else
            {
                string json = File.ReadAllText(usedPath + "/donjon.json");
                Models.Dungeon dungeonModel = JsonConvert.DeserializeObject<Models.Dungeon>(json);
                titleText.text = dungeonModel.Name;
                historyText.text = dungeonModel.History;
                image.sprite = IMG2Sprite.ConvertTextureToSprite(IMG2Sprite.LoadTexture(usedPath + "/" + dungeonModel.Image));
                editButton.GetComponentInChildren<Text>().text = "Edit";

                if (!isActive)
                    ActiveAllComponents(true);
                usedPath = string.Empty;
            }
        }
        
    }

    class DungeonSelector : MonoBehaviour, IPointerClickHandler
    {
        public string path;
        public DungeonPrinter outerReference;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                outerReference.usedPath = path;
            }
        }
    }
}