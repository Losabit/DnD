using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ValueChangedEvent<T>
{
    public T OldValue;
    public T CurrentValue;
    public string Tag;
    public delegate void OnVariableChangeDelegate(ValueChangedEvent<T> reference);

    private event OnVariableChangeDelegate OnVariableChange;

    public ValueChangedEvent(T value, OnVariableChangeDelegate func, string tag = "")
    {
        Value = OldValue = value;
        OnVariableChange = func;
        Tag = tag;
    }

    public T Value
    {
        set
        {
            if (CurrentValue.Equals(value)) return;
            CurrentValue = value;
            OnVariableChange(this);
            OldValue = CurrentValue;
        }
    }
}