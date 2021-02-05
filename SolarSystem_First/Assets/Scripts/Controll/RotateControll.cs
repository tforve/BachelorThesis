using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateControll : MonoBehaviour
{
    public float planetSpeed = 5f;
    public float sunSpeed = 5f;

    public int layerIndex;
    [SerializeField]
    private Light sun;

    private void Start()
    {
        SetLayer(this.gameObject, layerIndex);
    }

    private void FixedUpdate()
    {
        RotateCelectialObjects();
    }

    /// <summary>
    /// Use culling mask to apply light just to this.
    /// need to set Layers in Unity Insepctor
    /// </summary>
    void SetLayer(GameObject obj, int newLayer)
    {
        if (obj is null) return;
        
        obj.layer = newLayer;

        foreach (Transform child in obj.GetComponentsInChildren<Transform>())
        {
            if (child is null) continue;
            child.gameObject.layer = newLayer;            
        }
    }

    private void RotateCelectialObjects()
    {
        this.transform.Rotate(Vector3.down * (planetSpeed * Time.deltaTime));
        sun.transform.LookAt(this.transform);
    }
}
