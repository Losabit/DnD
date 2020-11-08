using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FightView : MonoBehaviour
{
    public int pointsAction;

    private int oldPointsAction = -1;
    private Text pointsActionText;

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if(child.tag == "ActionPoints")
            {
                pointsActionText = child.GetComponentInChildren<Text>();
            }
        }
    }

    private void Update()
    {
        if (oldPointsAction != pointsAction)
        {
            pointsActionText.text = pointsAction.ToString();
            oldPointsAction = pointsAction;
        }
    }
}

