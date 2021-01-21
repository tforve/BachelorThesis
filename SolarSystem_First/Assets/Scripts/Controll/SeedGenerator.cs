using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour
{
    public string stringSeed = "";
    public int seed;
    public bool useStringSeed;
    public bool randomizeSeed;

    private void Awake()
    {
        Random.InitState(seed);
    }

    private void OnValidate()
    {
        if (useStringSeed)
        {
            seed = stringSeed.GetHashCode();
        }

        if (randomizeSeed)
        {
            seed = Random.Range(0, 9999);
        }
    }
}
