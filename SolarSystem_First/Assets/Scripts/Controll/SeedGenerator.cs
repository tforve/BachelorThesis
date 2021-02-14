using UnityEngine;

public class SeedGenerator : MonoBehaviour
{
    public string stringSeed = "";
    public bool useStringSeed;
    public int seed;
    public bool randomizeSeed;

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

    /// <summary>
    /// get seed as int and set RNG on seed
    /// </summary>
    public void InputSeed(string seed)
    {
        this.seed = seed.GetHashCode();
        UnityEngine.Random.InitState(this.seed);
    }

    // --- DEBUG only ---
    private void OnValidate()
    {
        GenerateSeed();
    }

}
