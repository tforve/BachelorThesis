using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CPlanet/ShapeSettings")]
public class CShapeSettings : ScriptableObject
{
    public float planetRadius = 1.0f;
    public CNoiseSettings noiseSettings;
}
