using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Personnage
{
    public Models.Personnage model;
    public int controllerValue;

    public struct Initialization
    {
        public int numbers;
        public string fichePath;
    }

    public Personnage(Models.Personnage personnageModel, int value)
    {
        model = personnageModel;
        controllerValue = value;
    }
}
