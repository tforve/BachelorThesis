using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class SolarsystemBody : MonoBehaviour
{
    [HideInInspector]
    public float mass;                      // mass of planet

    [Header("Values of Celestial Body")]
    public float radius;                    // radius of planet
    public float surfaceGravity;            // surface Gravity to calculate mass
    public Vector3 startVelocity;           // give starting boost to planet
    public Vector3 currentVelocity;         // update velocity
    
    [SerializeField]
    private Rigidbody rb;
    private Transform mesh;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
    }

    // ------ RUNTIME: Move Planet --------

    /// <summary>
    /// calculate Velocity of planet based on law of universal gravitation (F = G *((m1*m2)/r^2))
    /// </summary>
    public void UpdateVelocity(SolarsystemBody[] planets, float timeSteps)
    {
        //loop through all planets
        foreach(var otherPlanet in planets)
        {   
            // don't attract your self
            if(otherPlanet != this)
            {
                //calculate distancen to each other | r^2 
                float sqrDistance = (otherPlanet.rb.position - this.rb.position).sqrMagnitude;
                // dir vector to each other
                Vector3 dir = (otherPlanet.rb.position - this.rb.position).normalized;
                // force (F = G *((m1*m2)/r^2))
                Vector3 force = dir * Universe.gravitationalConstant * ((this.mass * otherPlanet.mass) / sqrDistance);
                // acceleration = velocity / time
                Vector3 acceleration = force / this.mass;
                currentVelocity += acceleration * timeSteps;
            }
        }
    }      

    /// <summary>
    /// Update Position of this.Planet based on calculated velocity
    /// </summary>
    public void UpdatePosition(float time)
    {
        this.rb.position += currentVelocity * time;
    }

    ///<summary>
    ///calculate start Velocity based on curcular motion, v = square root (G*(M/r))
    ///</summary> 
    public void CalculateStartVelocity(SolarsystemBody[] planets)
    {
        foreach (var otherPlanet in planets)
        {
            // don't want to effect the sun
            if(otherPlanet != this) //&& this.CompareTag("Planet") || this.CompareTag("Moon"))
            {
                // calculate r
                float sqrDistance = (otherPlanet.rb.position - this.rb.position).magnitude;
                // ----------- 
                // dir vector to each other - has to be turned 90 degrees! or? REWORK HERE! Which Vector to rotate
                // ----------- 
                Vector3 dir = (otherPlanet.rb.position - this.rb.position).normalized;
                dir = Quaternion.Euler(-90.0f, 0.0f, -90.0f) * dir;
                // v = sqr(G*(M/r))
                Vector3 forceToStart = dir * (Mathf.Sqrt(Universe.gravitationalConstant * (otherPlanet.mass / sqrDistance)));
                startVelocity = forceToStart;
                currentVelocity = forceToStart;
            }
        }
    }

    // -------- MOSTLY FOR DEBUGGING PURPOSE: while in editor mode ---------

    /// <summary>
    /// Update mesh based on Radius of Planet
    /// </summary>
    void UpdateMesh()
    {
        mesh = this.transform.GetChild(0);
        mesh.localScale = new Vector3(radius, radius, radius);
    }

    /// <summary>
    /// caculate Mass based on radius and surfaceGravity. m = (g*r^2) / G || g = M*G/r^2
    /// </summary>
    void CalculateMass()
    {
        mass = (surfaceGravity*(radius*radius)) / Universe.gravitationalConstant;
    }

    private void OnValidate()
    {
        UpdateMesh();
        CalculateMass();
    }
}
