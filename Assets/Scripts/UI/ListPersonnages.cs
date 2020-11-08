using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using Siccity.GLTFUtility;

class ListPersonnages : MonoBehaviour
{
    public GameObject textObject;
    //public GameObject empty;

    private void Awake()
    {
        List<string> personnagesName = PersonnageModding.GetAllPersonnagesName();

        for (int j = 0; j < personnagesName.Count; j++)
        {
            Transform textTransform = Instantiate(textObject.transform);
            //not work (unity problem ?)
            RectTransform textRectTransform = textTransform.gameObject.GetComponent<RectTransform>();
            textRectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            Text text = textTransform.gameObject.GetComponent<Text>();
            text.text = personnagesName[j];
            textTransform.SetParent(transform);
        }
    }

    IEnumerator GetJsonFile()
    {
         UnityWebRequest www = UnityWebRequest.Get("C:/Users/quent/Documents/Unity/DnD/Assets/Resources/Personnages/test.json");
        //UnityWebRequest www = UnityWebRequest.Get("http://localhost/DnD/Assets/test.json");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            AssetBundle bundle = null;//www.;
            if (bundle != null)
            {
                Debug.Log("in bitch");
                string rootAssetPath = bundle.GetAllAssetNames()[0];
                GameObject arObject = Instantiate(bundle.LoadAsset(rootAssetPath)) as GameObject;
               
            }
            else
            {
                Debug.Log("Not a valid asset bundle");
            }
        }
    }

/*
    void Get3DModel()
    {
        Mesh myMesh = FastObjImporter.Instance.ImportFile("C:/Users/quent/Documents/Unity/DnD/Assets/Resources/Personnages/Models/personnage.obj");
        MeshRenderer renderer = empty.AddComponent<MeshRenderer>();
        MeshFilter filter = empty.AddComponent<MeshFilter>();
        filter.mesh = myMesh;
        
        //GameObject wrapper = new GameObject
        //{
        //    name = "Model"
        //};
        //string path = "http://localhost/DnD/Assets/personnage.glb";
        //string savePath = "C:/Users/quent/Documents/Unity/DnD/Assets/personnage.glb";

        //UnityWebRequest www = UnityWebRequest.Get(path);
        //www.downloadHandler = new DownloadHandlerFile(savePath);
        //yield return www.SendWebRequest();

        //if (www.isNetworkError || www.isHttpError)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    Debug.Log(www.downloadHandler.text);
        //    ResetWrapper(wrapper);
        //    GameObject model = Importer.LoadFromFile(savePath);
        //    model.transform.SetParent(wrapper.transform);
        //}
       
    }

    void ResetWrapper(GameObject wrapper)
    {
        if (wrapper != null)
        {
            foreach (Transform trans in wrapper.transform)
            {
                Destroy(trans.gameObject);
            }
        }
    }
*/
}

