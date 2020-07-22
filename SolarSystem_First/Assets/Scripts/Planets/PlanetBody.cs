using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlanetBody : MonoBehaviour
{
    public float mass;              // mass of planet
    public float radius;            // radius of planet
    public Vector3 startVelocity;
    private Vector3 currentVelocity;


    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        currentVelocity = startVelocity;
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
                //calculate distancen to each other
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

    /// <summary>
    /// Update Position of this.Planet based on calculated velocity
    /// </summary>
    public void UpdatePosition(float time)
    {
        this.rb.position += currentVelocity * time;
    }

}
