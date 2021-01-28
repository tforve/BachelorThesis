using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour
{
    public string stringSeed = "";
    public bool useStringSeed;
    public int seed;
    public bool randomizeSeed;

    //private void Awake()
    //{
    //    Random.InitState(seed);
    //}

    private void OnValidate()
    {
        if (useStringSeed)
        {
            seed = stringSeed.GetHashCode();
        }

        if (randomizeSeed)
        {
            seed = Random.Range(0, 99999);
        }
    }

}
