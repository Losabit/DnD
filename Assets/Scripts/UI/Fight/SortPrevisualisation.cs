using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SortPrevisualisation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 personnagePosition;
    public Personnage personnage;
    public Models.Sorts sort;
    public bool readyToLaunchSort = false;
    public bool canPrevisualize = true;
    public Vector3? sortLaunchedAt = null;
    public List<Transform> hitedCases = new List<Transform>();

    private bool canClean = true;
    private bool mouse_over = false;
    private GameObject mapObject;
    private Material material;
    private Material sortMaterial;
    private Vector3 oldPersonnagePosition = new Vector3();
    private List<Vector3> touchedCases = new List<Vector3>();
    private List<Material> defaultMaterials = new List<Material>();
    private List<Transform> childs = new List<Transform>();

    private List<Transform> oldsHitedCases = new List<Transform>();
    private Vector3 oldHitPosition = new Vector3(666, 666, 666);
    private List<Material> hitedMaterials = new List<Material>();

    //garder seuleument les positions intéressantes
    // ne pas parcourir les 800 objects a chaque mais le faire méthodiquement ? // 0 -> 0,0 // 1 -> 0,1
    void Start()
    {
        readyToLaunchSort = false;
        mapObject = GameObject.FindGameObjectsWithTag("Map")[0];
        material = (Material)Resources.Load("Fight/OverCaseMaterial");
        sortMaterial = (Material)Resources.Load("Fight/AttackedCasesMaterial");
        if (personnage == null || personnagePosition == null)
            throw new Exception("perso not defined");
    }

    void Update()
    {
        if (oldPersonnagePosition != personnagePosition)
        {
            touchedCases = SortModding.GetTouchedCases(sort, personnagePosition);
            oldPersonnagePosition = personnagePosition;
        }

        if (!mouse_over && readyToLaunchSort)
        {
            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform.position != oldHitPosition)
                {
                    if (oldsHitedCases.Count > 0)
                    {
                        for (int i = 0; i < oldsHitedCases.Count; i++)
                        {
                            oldsHitedCases[i].GetComponent<MeshRenderer>().material = hitedMaterials[i];
                        }
                        oldsHitedCases.Clear();
                        hitedMaterials.Clear();
                    }
                    else if (childs.Where(x => x.position == hit.collider.transform.position).Count() > 0)
                    {
                        List<Vector3> vectors = SortModding.GetAttackCases(hit.collider.transform.position, sort);
                        for (int i = 0; i < mapObject.transform.childCount; i++)
                        {
                            Transform child = mapObject.transform.GetChild(i);
                            if (vectors.Where(x => x == child.position).ToList().Count > 0 && child.tag == "MouseOver")
                            {
                                hitedMaterials.Add(child.GetComponent<MeshRenderer>().material);
                                child.GetComponent<MeshRenderer>().material = sortMaterial;
                                oldsHitedCases.Add(child);
                            }
                        }
                        oldHitPosition = hit.collider.transform.position;
                    }
                    else
                    {
                        oldHitPosition = new Vector3(666, 666, 666);
                    }
                }
                //for (int i = 0; i < childs.Count; i++)
                //{
                //    if (vectors.Where(x => x == childs[i].position).ToList().Count > 0)
                //    {
                //        childs[i].GetComponent<MeshRenderer>().material = sortMaterial;
                //    }
                //}
            }

            if (Input.GetMouseButtonDown(0) && childs.Where(x => x.position == hit.collider.transform.position).Count() > 0)
            {
                hitedCases = oldsHitedCases.Clone();
                sortLaunchedAt = hit.collider.transform.position;
                readyToLaunchSort = false;
                Clean();
            }
        }

        

        if (mouse_over && Input.GetMouseButtonDown(0))
        {
            readyToLaunchSort = true;
            canClean = false;

        }
        else if (Input.GetMouseButtonDown(1))
        {
            Clean();
            canClean = true;
            readyToLaunchSort = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (childs.Count == 0 && canPrevisualize)
        {
            mapObject.transform.GetChild((int)personnagePosition.x + (int)personnagePosition.z * 20);
            for (int i = 0; i < mapObject.transform.childCount; i++)
            {
                Transform child = mapObject.transform.GetChild(i);
                if (touchedCases.Where(x => new Vector3(x.x, 1f, x.z) == child.position).ToList().Count > 0 &&
                    child.tag == "MouseOver")
                {
                    childs.Add(child);
                    defaultMaterials.Add(child.GetComponent<MeshRenderer>().material);
                    child.GetComponent<MeshRenderer>().material = material;
                }
            }
            canClean = true;
        }
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canClean)
        {
            Clean();
            canClean = false;
        }
        mouse_over = false;
    }

    private float CaseDistance(Vector3 start, Vector3 end)
    {
        return Mathf.Abs(start.x - end.x) +
            Mathf.Abs(start.y - end.y) +
            Mathf.Abs(start.z - end.z);
    }

    private void Clean()
    {
        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].GetComponent<MeshRenderer>().material = defaultMaterials[i];
        }

        for (int i = 0; i < oldsHitedCases.Count; i++)
        {
            if (!childs.Contains(oldsHitedCases[i]))
                oldsHitedCases[i].GetComponent<MeshRenderer>().material = hitedMaterials[i];
        }

        childs.Clear();
        defaultMaterials.Clear();
        oldsHitedCases.Clear();
        hitedMaterials.Clear();
    }
}
