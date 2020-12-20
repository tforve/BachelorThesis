using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMinMax
{
    public float Min { get; private set; }
    public float Max { get; private set; }

    public CMinMax()
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }
    
    public void AddValue(float value)
    {
        if (value > Max) Max = value;
        if (value < Min) Min = value;
    }
}
