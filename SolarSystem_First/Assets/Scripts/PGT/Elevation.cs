using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// keep track of all elevations for all the vertices of the planet
/// </summary>
public class Elevation
{
    public float minElevation { get; private set; }         // lowest point
    public float maxElevation { get; private set; }         // highest point 

    public Elevation()
    {
        minElevation = float.MaxValue;
        maxElevation = float.MinValue;
    }
    
    public void AddValue(float value)
    {
        if (value > maxElevation) maxElevation = value;
        if (value < minElevation) minElevation = value;
    }
}
