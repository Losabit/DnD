using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

class TexturePrinter : MonoBehaviour
{
    public GameObject imagePrefab;
    public Texture2D usedTexture = null;
    public string fileNameOfTexture = string.Empty;

    private void Start()
    {
        string[] texturesPath = Directory.GetFiles("C:/Users/quent/Documents/Unity/DnD/Assets/Resources/Map/Textures");
        foreach (string path in texturesPath)
        {
            if (path.EndsWith(".meta"))
                continue;
            string filename = Path.GetFileName(path);
            InstanciateSprite(IMG2Sprite.LoadTexture(path),
                filename.Remove(filename.LastIndexOf(".")));
        }
    }

    private void InstanciateSprite(Texture2D texture, string filename)
    {
        Image image = imagePrefab.GetComponent<Image>();
        image.sprite = IMG2Sprite.ConvertTextureToSprite(texture);

        Transform transformObject = Instantiate(imagePrefab.transform);
        TextureSelector textureSelector = transformObject.gameObject.AddComponent<TextureSelector>();
        textureSelector.texture = texture;
        textureSelector.filename = filename;
        textureSelector.outerReference = this;
        transformObject.SetParent(transform, false);
    }

    Ray ray;
    RaycastHit hit;
    private bool canModifyTexture = false;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            canModifyTexture = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            canModifyTexture = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            usedTexture = null;
            fileNameOfTexture = string.Empty;
            canModifyTexture = false;
        }

        if (usedTexture != null && canModifyTexture)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                hit.collider.GetComponent<MeshRenderer>().material.mainTexture = usedTexture;
                hit.collider.gameObject.GetComponent<MapModding.ItemStorage>().texture = "Map/Textures/" + fileNameOfTexture;
            }
        }
    }

    class TextureSelector : MonoBehaviour, IPointerClickHandler
    {
        public Texture2D texture;
        public string filename;
        public TexturePrinter outerReference;

        public void OnPointerClick(PointerEventData eventData)
        {
            outerReference.fileNameOfTexture = filename;
            outerReference.usedTexture = texture;
        }
    }
}