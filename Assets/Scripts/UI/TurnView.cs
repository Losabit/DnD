using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnView : MonoBehaviour
{
    public List<string> personnagesName;
    public GameObject turnObject;

    private bool turnViewInstanced = false;
    public EventSender turnSender;

    void Start()
    {
        turnSender = GameObject.FindObjectOfType<EventSender>();
        turnSender.OnVariableChange += VariableChangeHandler;
    }

    private void VariableChangeHandler(int newVal, int oldVal)
    {
        Transform oldPlayer = transform.GetChild(oldVal);
        Transform newPlayer = transform.GetChild(newVal);

        Text newtext = newPlayer.gameObject.GetComponentInChildren<Text>();
        newtext.color = Color.red;
        Text oldtext = oldPlayer.gameObject.GetComponentInChildren<Text>();
        oldtext.color = Color.black;
    }

    private void Update()
    {
        if (personnagesName != null && !turnViewInstanced)
        {
            for (int j = 0; j < personnagesName.Count; j++)
            {
                Transform textTransform = Instantiate(turnObject.transform);
                Text text = textTransform.gameObject.GetComponentInChildren<Text>();
                text.text = personnagesName[j];
                if(turnSender.MyValue == j)
                {
                    text.color = Color.red;
                }
                textTransform.SetParent(transform);
            }
            turnViewInstanced = true;
        }
    }
}

