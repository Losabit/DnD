using System.Collections.Generic;
using UnityEngine;

//https://forum.unity.com/threads/variable-listener.468721/
public class EventSender : MonoBehaviour
{
    private Dictionary<int, Personnage.Initialization> _myFloat = new Dictionary<int, Personnage.Initialization>();

    public event OnVariableChangeDelegate OnVariableChange;
    public delegate void OnVariableChangeDelegate(Dictionary<int, Personnage.Initialization> newVal);

    public Dictionary<int, Personnage.Initialization> MyFloat
    {
        get
        {
            return _myFloat;
        }
        set
        {
            if (_myFloat.Equals(value)) return;
            _myFloat = value;
            if (OnVariableChange != null)
                OnVariableChange(_myFloat);
        }
    }
}