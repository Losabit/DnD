using UnityEngine;

//https://forum.unity.com/threads/variable-listener.468721/
public class EventSender : MonoBehaviour
{
    private int _myValue = 0;

    public event OnVariableChangeDelegate OnVariableChange;
    public delegate void OnVariableChangeDelegate(int newVal, int oldVal);

    public int MyValue
    {
        get
        {
            return _myValue;
        }
        set
        {
            if (_myValue.Equals(value)) return;
            int buff = _myValue;
            _myValue = value;
            if (OnVariableChange != null)
                OnVariableChange(_myValue, buff);
        }
    }
}