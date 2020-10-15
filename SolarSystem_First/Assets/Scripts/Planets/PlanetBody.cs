using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlanetBody : MonoBehaviour
{
    public float mass;                  // mass of planet
    public float radius;                // radius of planet
    public Vector3 startVelocity;       // give starting boost to planet
    public Vector3 currentVelocity;     // update velocity

    


    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        //currentVelocity = startVelocity;
    }


    /// <summary>
    /// calculate Velocity of planet based on law of universal gravitation (F = G *((m1*m2)/r^2))
    /// </summary>
    public void UpdateVelocity(PlanetBody[] planets, float timeSteps)
    {
        //loop through all planets
        foreach(var p in planets)
        {   
            // don't attract your self
            if(p != this)
            {
                //calculate distancen to each other | r^2 
                float sqrDistance = (p.rb.position - this.rb.position).sqrMagnitude;
                // dir vector to each other
                Vector3 dir = (p.rb.position - this.rb.position).normalized;
                // force (F = G *((m1*m2)/r^2))
                Vector3 force = dir * Universe.gravitationalConstant * ((mass * p.mass) / sqrDistance);
                // acceleration = velocity / time
                Vector3 acceleration = force / mass;
                currentVelocity += acceleration * timeSteps;
            }
        }
    }

    // calculate start Velocity
    public void CalculateStartVelocity(PlanetBody[] planets)
    {
        foreach (var p in planets)
        {
            if(p != this && this.CompareTag("Planet"))
            {
                // calculate r
                float sqrDistance = (p.rb.position - this.rb.position).magnitude;
                // ----------- 
                // dir vector to each other - has to be turned 90 degrees! or? REWORK HERE! Which Vector to rotate
                // ----------- 
                Vector3 dir = (p.rb.position - this.rb.position).normalized;
                dir = Quaternion.Euler(-90.0f, -90.0f, 0.0f) * dir;
                // v = sqr(G*(M/r))
                Vector3 forceToStart = dir * (Mathf.Sqrt(Universe.gravitationalConstant * (p.mass / sqrDistance)));
                startVelocity = forceToStart;
                currentVelocity = forceToStart;
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

}
