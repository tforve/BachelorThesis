using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugOrbit : MonoBehaviour
{

    public int numSteps = 1000;             // how many steps to calculate in future
    public float timeSteps = 0.1f;


    [Header("Relative to Body")]
    public PlanetBody centralBody;
    public bool relativeToCentralBody = true;
    public bool usePhysicsTimeStep = true;
    public bool useThickLines;
    public float width = 100;


    public bool drawInPlayMode = false;


    private void Start()
    {
        if (Application.isPlaying)
        {
            HideOrbits();
        }
    }

    private void Update()
    {
        if(drawInPlayMode || !Application.isPlaying)
        {
            DrawOrbits();
        }
    }

    /// <summary>
    /// Draw Orbits with Linerenderer
    /// </summary>
    void DrawOrbits()
    {
        // all bodies 
        PlanetBody[] bodies = FindObjectsOfType<PlanetBody>();
        var simulatedBodies = new SimulatedBody[bodies.Length];
        var drawPoints = new Vector3[bodies.Length][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = Vector3.zero;

        // Init simulated bodies
        for (int i = 0; i < simulatedBodies.Length; i++)
        {
            simulatedBodies[i] = new SimulatedBody(bodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            //
            if(bodies[i] == centralBody && relativeToCentralBody)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = simulatedBodies[i].position;
            }
        }

        // Simulate
        for (int step = 0; step < numSteps; step++)
        {
            // celectial body which is seen as the center
            Vector3 refernceBody = relativeToCentralBody ? simulatedBodies[referenceFrameIndex].position : Vector3.zero;

            // loop through simulated Bodies and calculate all accelerations
            for (int i = 0; i < simulatedBodies.Length; i++)
            {
                simulatedBodies[i].velocity += CalculateAcceleration(i, simulatedBodies) * timeSteps;
            }

            // update Positions
            for (int i = 0; i < simulatedBodies.Length; i++)
            {
                // calculate new Position for all bodies
                Vector3 newPos = simulatedBodies[i].position + simulatedBodies[i].velocity * timeSteps;
                simulatedBodies[i].position = newPos;

                if (relativeToCentralBody)
                {
                    var referenceFrameOffset = referenceBodyInitialPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToCentralBody && i == referenceFrameIndex)
                {
                    newPos = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = newPos;
            }


            // draw paths
            for (int bodyIndex = 0; bodyIndex < simulatedBodies.Length; bodyIndex++)
            {
                // set pathColor to material color
                var pathColor = bodies[bodyIndex].gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color;

                if(useThickLines)
                {
                    var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();

                    lineRenderer.enabled = true;
                    lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                    lineRenderer.SetPositions(drawPoints[bodyIndex]);
                    lineRenderer.startColor = pathColor;
                    lineRenderer.endColor = pathColor;
                    lineRenderer.widthMultiplier = width;
                }
                else
                {
                    for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
                    {
                        Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColor);
                    }

                    // Hide renderer
                    var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                    if (lineRenderer)
                    {
                        lineRenderer.enabled = false;
                    }
                }
                
            }
        }

    }

    /// <summary>
    /// Hide Orbits in playmode
    /// </summary>
    void HideOrbits()
    {
        PlanetBody[] bodies = FindObjectsOfType<PlanetBody>();

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < bodies.Length; bodyIndex++)
        {
            var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
            lineRenderer.positionCount = 0;
        }
    }

    /// <summary>
    /// Calculate Acceleration of all celectial bodies
    /// </summary>
    /// <param name="i"></param>
    /// <param name="simulatedBodies"></param>
    /// <returns></returns>
    private Vector3 CalculateAcceleration(int i, SimulatedBody[] simulatedBodies)
    {
        Vector3 acceleration = Vector3.zero;

        for (int j = 0; j < simulatedBodies.Length; j++)
        {
            if( i == j)
            {
                //calculate distancen to each other | r^2 
                float sqrDistance = (simulatedBodies[j].position - simulatedBodies[i].position).sqrMagnitude;
                // dir vector to each other
                Vector3 direction = (simulatedBodies[j].position - simulatedBodies[i].position).normalized;                
                // acceleration
                acceleration += (direction * Universe.gravitationalConstant * simulatedBodies[j].mass) / sqrDistance;
            }
        }
        return acceleration;
    }


    private void OnValidate()
    {
        if (usePhysicsTimeStep)
        {
            timeSteps = Universe.timeSteps;
        }
    }


    // ----- CLASS: to simulate and precalculate celestial bodies -------
    class SimulatedBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public SimulatedBody(PlanetBody body)
        {
            position = body.transform.position;
            velocity = body.startVelocity;
            mass = body.mass;
        }
    }
}
