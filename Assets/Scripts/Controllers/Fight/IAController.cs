using UnityEngine;
using System.Collections.Generic;
using Models;
using System.Linq;

public class IAController : FightPlayController
{
    private Material material;
    private GameObject mapObject;
    private Transform sortTransform;
    private Vector2 ennemiPosition;
    private bool newTurn = false;

    void Start()
    {
        Initialize();
        position = new Vector2((int)transform.position.x, (int)transform.position.z);
        mapObject = GameObject.FindGameObjectsWithTag("Map")[0];
        ennemiPosition = GetEnnemiPosition(1).Value;
    }

    void Update()
    {
        PlayAnimations();
        if (newTurn && canPlay)
        {
            ennemiPosition = GetEnnemiPosition(1).Value;
            newTurn = false;
        }

        if (personnage.currentActionPoints == 0)
        {
            personnage.currentActionPoints = personnage.model.ActionPoints;
            newTurn = true;
            canPlay = false;
        }

        if (canPlay)
        {
            ennemiPosition = GetEnnemiPosition(1).Value;
            int distance = GetDistance(position, ennemiPosition);
            Sorts sort = GetBestSort(personnage, distance);
            if (sort != null)
            {
                personnage.currentActionPoints -= sort.ActionPoints;
                Debug.Log("launch");
            }
            else
            {
                List<Vector2> path = AStar.execute(map, ennemiPosition, position);
                for (int i = 0; i < path.Count; i++)
                {
                    if (personnage.currentActionPoints == 0)
                    {
                        break;
                    }
                    Move((int)path[i].x, (int)path[i].y);
                }
            }
        }

    }

    public Vector2? GetEnnemiPosition(int ennemiValue)
    {  
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == ennemiValue)
                    return new Vector2(j, i);
            }
        }
        return null;
    }

    public Sorts GetBestSort(Personnage personnage, int distance)
    {
        List<Sorts> usableSorts = new List<Sorts>();
        for (int i = 0; i < personnage.model.Sorts.Count; i++)
        {
            Sorts sort = personnage.model.Sorts[i];
            if (distance >= sort.MinimumScope && distance <= sort.MaximumScope &&
                personnage.currentActionPoints >= sort.ActionPoints)
            {
                usableSorts.Add(sort);
            }
        }
        if (usableSorts.Count == 0)
            return null;
        return usableSorts.OrderBy(x => x.Damage).ToList()[0];
    }
}


