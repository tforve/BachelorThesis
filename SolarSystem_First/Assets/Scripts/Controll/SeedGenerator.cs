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

    public void GenerateSeed()
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

    private void OnValidate()
    {
        GenerateSeed();
    }

}
