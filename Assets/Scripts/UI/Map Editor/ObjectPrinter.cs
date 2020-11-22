using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class ObjectPrinter : MonoBehaviour
{
    public GameObject imagePrefab;
    public GameObject usedObject = null;
    public string fileNameOfObject = string.Empty;
    public GameObject map;

    public void Start()
    {
        string[] objectsPath = Directory.GetFiles("C:/Users/quent/Documents/Unity/DnD/Assets/Resources/Map/Objects");
        foreach (string path in objectsPath)
        {
            if (path.EndsWith(".meta"))
                continue;
            string filename = Path.GetFileName(path);
            filename = filename.Remove(filename.LastIndexOf("."));
            GameObject obj = Resources.Load<GameObject>("Map/Objects/" + filename);

            string snapshotPath = "C:/Users/quent/Documents/Unity/DnD/Assets/Resources/Map/Objects/Snapshots/" + filename + ".png";
            if (!File.Exists(snapshotPath))
            {
                GameObject snapshotCameraGO = new GameObject(filename + " camera");
                SnapshotCamera snapshotCamera = SnapshotCamera.MakeSnapshotCamera(snapshotCameraGO);
                FileInfo info = SnapshotCamera.SavePNG(snapshotCamera.TakePrefabSnapshot(obj), 
                    filename, "C:/Users/quent/Documents/Unity/DnD/Assets/Resources/Map/Objects/Snapshots");
               Destroy(snapshotCameraGO);
            }
            InstanciateSprite(IMG2Sprite.LoadTexture(snapshotPath), obj, filename);
        }
    }

    Ray ray;
    RaycastHit hit;
    private bool canModifyObject = false;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            canModifyObject = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            canModifyObject = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            usedObject = null;
            canModifyObject = false;
            fileNameOfObject = string.Empty;
        }

        if (usedObject != null && canModifyObject)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                usedObject.transform.localPosition = hit.collider.transform.localPosition;
                MapModding.ItemStorage oldStorage = hit.collider.gameObject.GetComponent<MapModding.ItemStorage>();
                Transform transformObject = Instantiate(usedObject.transform);
                MapModding.ItemStorage storage = transformObject.gameObject.AddComponent<MapModding.ItemStorage>();
                storage.obj = "Map/Objects/" + fileNameOfObject;
                storage.x = oldStorage.x;
                storage.z = oldStorage.z;
                transformObject.SetParent(map.transform, false);
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void InstanciateSprite(Texture2D texture, GameObject obj, string filename)
    {
        Image image = imagePrefab.GetComponent<Image>();
        image.sprite = IMG2Sprite.ConvertTextureToSprite(texture);

        Transform transformObject = Instantiate(imagePrefab.transform);
        ObjectSelector objectSelector = transformObject.gameObject.AddComponent<ObjectSelector>();
        objectSelector.obj = obj;
        objectSelector.filename = filename;
        objectSelector.outerReference = this;
        transformObject.SetParent(transform, false);
    }

    class ObjectSelector : MonoBehaviour, IPointerClickHandler
    {
        public GameObject obj;
        public string filename;
        public ObjectPrinter outerReference;

        public void OnPointerClick(PointerEventData eventData)
        {
            outerReference.usedObject = obj;
            outerReference.fileNameOfObject = filename;
        }
    }
}