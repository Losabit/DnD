using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Personnage
{
    public Models.Personnage model;
    public int controllerValue;
    public int currentActionPoints;

    public struct Initialization
    {
        public int numbers;
        public string fichePath;
    }

    public Personnage(Models.Personnage personnageModel, int value)
    {
        model = personnageModel;
        controllerValue = value;
        currentActionPoints = personnageModel.ActionPoints;
    }

    public GameObject LaunchSort(int choosenSort)
    {
        if (currentActionPoints < model.Sorts[choosenSort].ActionPoints)
            return null;

        currentActionPoints -= model.Sorts[choosenSort].ActionPoints;
        GameObject sortObject = (GameObject)Resources.Load(model.Sorts[choosenSort].Animation);
        if (sortObject == null)
        {
            throw new System.Exception("can't load animation : " + model.Sorts[choosenSort].Animation);
        }
        return sortObject;
    }

    public bool UpdateLaunchedSort(SortsPrinter sortsPrinter, Transform sortTransform)
    {
        if ((int)sortsPrinter.sortsLaunchedAt.x == (int)sortTransform.position.x && (int)sortsPrinter.sortsLaunchedAt.z == (int)sortTransform.position.z)
        {
            sortTransform.position = sortsPrinter.sortsLaunchedAt;
            return true;
        }
        else
        {
            float vitesse = 0.1f;
            float newPositionX = sortTransform.position.x;
            float newPositionZ = sortTransform.position.z;

            if (sortsPrinter.sortsLaunchedAt.x > sortTransform.position.x)
            {
                newPositionX += vitesse;
            }
            else if (sortsPrinter.sortsLaunchedAt.x < sortTransform.position.x)
            {
                newPositionX -= vitesse;
            }

            if (sortsPrinter.sortsLaunchedAt.z > sortTransform.position.z)
            {
                newPositionZ += vitesse;
            }
            else if (sortsPrinter.sortsLaunchedAt.z < sortTransform.position.z)
            {
                newPositionZ -= vitesse;
            }
            sortTransform.position = new Vector3(newPositionX, sortTransform.position.y, newPositionZ);
        }
        return false;
    }

    public void EndTurn()
    {
        currentActionPoints = model.ActionPoints;
    }
}
