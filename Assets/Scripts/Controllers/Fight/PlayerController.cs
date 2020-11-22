﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Models;
using System.Collections;

//  simplifier le code avant d'écrire IAController (en utilisant Personnage et FightPlayController) // Créer une classe MovementPrevisualisation
//  Utiliser Les sorts (avec Animation) et faire les fiches de personnages, la vie...
public class PlayerController : FightPlayController
{
    public GameObject view;

    private Material material;
    private List<Transform> hits = new List<Transform>();
    private bool clean = true;
    private GameObject mapObject;
    private SortsPrinter sortsPrinter = null;
    private FightView fightView;
    private Transform sortTransform;

    void Start()
    {
        mapObject = GameObject.FindGameObjectsWithTag("Map")[0];
        material = (Material)Resources.Load("Fight/OverCaseMaterial");
        position = new Vector2(transform.localPosition.x, transform.localPosition.z);
    }

    Ray ray;
    RaycastHit hit;
    void Update()
    {
        if (view != null && sortsPrinter == null)
        {
            sortsPrinter = view.GetComponentInChildren<SortsPrinter>();
            sortsPrinter.personnage = personnage;
            sortsPrinter.personnagePosition = transform.position;
            sortsPrinter.map = map;
            fightView = view.GetComponent<FightView>();
            fightView.playerController = this;
            fightView.pointsAction = personnage.currentActionPoints;
        }

        if (sortsPrinter != null && sortsPrinter.sortChoosen != -1 && sortsPrinter.sortsLaunched.Count > 0
            && canPlay)
        {
            GameObject sortObject = personnage.LaunchSort(sortsPrinter.sortChoosen);
            sortTransform = Instantiate(sortObject.transform);
            sortTransform.position = transform.position;
            sortTransform.SetParent(transform.parent, false);

            fightView.pointsAction = personnage.currentActionPoints;
            sortsPrinter.sortsLaunched.Clear();
        }

        if (sortTransform != null)
        {
            if (personnage.UpdateLaunchedSort(sortsPrinter, sortTransform))
            {
                sortTransform = null;
            }
        }

        if (personnage != null && canPlay && map != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)
                && material != null && hit.collider.tag == "MouseOver")
            {
                int distance = (int)GetDistance(transform.localPosition, hit.collider.transform.position);
                if (hits.Count != 0 && hit.collider.transform.position != hits[0].position)
                {
                    CleanMap();
                }

                if (hit.collider.transform.position == transform.localPosition)
                {
                    //Debug.Log("touch personnage");
                }
                else if (hits.Count == 0 && distance <= personnage.currentActionPoints)
                {
                    List<Vector2> path = AStar.execute(map, transform.localPosition, hit.collider.transform.position);

                    if (path.Count <= personnage.currentActionPoints)
                    {
                        for (int i = 0; i < mapObject.transform.childCount; i++)
                        {
                            Transform child = mapObject.transform.GetChild(i);
                            if (path.Where(x => new Vector3(x.x, 1f, x.y) == child.position).ToList().Count > 0 &&
                                child.GetComponent<MeshRenderer>().material.name.Replace(" (Instance)", "") != material.name)
                            {
                                child.GetComponent<MeshRenderer>().materials = new Material[] { child.GetComponent<MeshRenderer>().material, material }; ;
                                if (hit.collider.transform.position != child.position)
                                    hits.Add(child);
                                else
                                    hits.Insert(0, child);
                                clean = false;

                                if (hits.Count == path.Count)
                                    break;
                            }
                        }
                    }
                }

                if (Input.GetMouseButtonDown(0)
                     && distance <= personnage.currentActionPoints)
                {
                   // transform.localPosition = hit.collider.transform.position;
                    Move((int)hit.collider.transform.position.x, (int)hit.collider.transform.position.z);
                   // personnage.currentActionPoints -= distance;
                    fightView.pointsAction = personnage.currentActionPoints;
                    if (sortsPrinter == null)
                        Debug.Log("sortPrinter not defined");
                    sortsPrinter.personnagePosition = transform.position;
                    CleanMap();
                }
            }
        }

        if (!canPlay)
        {
            if (!clean)
                CleanMap();
            personnage.currentActionPoints = personnage.model.ActionPoints;
            fightView.pointsAction = personnage.model.ActionPoints;
        }
    }

   

    private void CleanMap()
    {
        for (int i = 0; i < hits.Count; i++)
        {
            hits[i].GetComponent<MeshRenderer>().materials = new Material[] { hits[i].GetComponent<MeshRenderer>().materials[0] };
        }
        hits.Clear();
        clean = true;
    }


   
}