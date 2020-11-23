using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon
{
    public List<int> vertices;

    public Polygon(int a, int b, int c)
    {
        vertices = new List<int>() { a, b, c };
    }

}
