using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CINoiseFilter
{
    float Evaluate(Vector3 point);
}
