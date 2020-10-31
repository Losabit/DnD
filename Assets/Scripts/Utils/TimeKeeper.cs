using System;
using System.Collections;

public class TimeKeeper
{
    public float WaitTime;
    public float value;
    public Func<IEnumerator> FunctionToExecute;

    public TimeKeeper(float waitTime, Func<IEnumerator> func)
    {
        WaitTime = waitTime;
        FunctionToExecute = func;
        value = 0;
    }

    public IEnumerator ReturnOrIncrease(float increaseValue)
    {
        if ((value += increaseValue) > WaitTime)
        {
            value = 0;
            yield return FunctionToExecute();
        }
        yield return null;
    }
}