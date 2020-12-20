using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugOrbit : MonoBehaviour
{
    public int numSteps = 1500;
    public float timeStep = 0.1f;
    public bool usePhysicsTimeStep = false;

    [Header("Relative to Body")]
    public SolarsystemBody centralBody;
    public bool relativeToCentralBody = true;

    [Header("Drawing")]
    public bool drawInPlayMode = false;
    public bool useThickLines = true;
    public float width = 50;

    private SolarsystemBody[] bodies;


    void Start()
    {
        if (Application.isPlaying)
        {
            HideOrbits();
        }

        
    }

    void Update()
    {
        
        if (drawInPlayMode || !Application.isPlaying)
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
        //SimulateBody[] bodies = FindObjectsOfType<SimulateBody>();
        var simulatedBodies = new SimulatedBody[bodies.Length];
        var drawPoints = new Vector3[bodies.Length][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = Vector3.zero;

        // Init simulated bodies
        for (int i = 0; i < simulatedBodies.Length; i++)
        {
            simulatedBodies[i] = new SimulatedBody(bodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (bodies[i] == centralBody && relativeToCentralBody)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = simulatedBodies[i].position;
            }
        }

        // Simulate
        for (int step = 0; step < numSteps; step++)
        {
            // celectial body which is seen as the center
            Vector3 referenceBodyPosition = (relativeToCentralBody) ? simulatedBodies[referenceFrameIndex].position : Vector3.zero;

            // loop through simulated Bodies and calculate all accelerations
            for (int i = 0; i < simulatedBodies.Length; i++)
            {
                simulatedBodies[i].velocity += CalculateAcceleration(i, simulatedBodies) * timeStep;
            }
            // Update positions
            for (int i = 0; i < simulatedBodies.Length; i++)
            {
                // calculate new Position for all bodies
                Vector3 newPos = simulatedBodies[i].position + simulatedBodies[i].velocity * timeStep;
                simulatedBodies[i].position = newPos;

                if (relativeToCentralBody)
                {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToCentralBody && i == referenceFrameIndex)
                {
                    newPos = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = newPos;
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < simulatedBodies.Length; bodyIndex++)
        {
            // set pathColor to material color
            var pathColour = bodies[bodyIndex].gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color;

            if (useThickLines)
            {
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
      
                lineRenderer.enabled = true;
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions(drawPoints[bodyIndex]);
                lineRenderer.startColor = pathColour;
                lineRenderer.endColor = pathColour;
                lineRenderer.widthMultiplier = width;                
            }
            else
            {
                for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
                {
                    Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour);
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

    /// <summary>
    /// Hide Orbits in playmode
    /// </summary>
    void HideOrbits()
    {
        SolarsystemBody[] bodies = FindObjectsOfType<SolarsystemBody>();

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
    Vector3 CalculateAcceleration(int i, SimulatedBody[] simulatedBodies)
    {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < simulatedBodies.Length; j++)
        {
            if (i == j)
            {
                continue;
            }
            // dir vector to each other
            Vector3 forceDir = (simulatedBodies[j].position - simulatedBodies[i].position).normalized;
            //calculate distancen to each other | r^2 
            float sqrDst = (simulatedBodies[j].position - simulatedBodies[i].position).sqrMagnitude;
            // acceleration
            acceleration += forceDir * Universe.gravitationalConstant * simulatedBodies[j].mass / sqrDst;
        }
        return acceleration;
    }


    /// <summary>
    /// Turn on and off Orbits of celectial Bodies
    /// </summary>
    public void ShowOrbit()
    {
        ResetOrbit();
        drawInPlayMode = drawInPlayMode ? false : true;
        useThickLines = useThickLines ? false : true;
    }

    void ResetOrbit()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            var lineRenderer = bodies[i].gameObject.GetComponentInChildren<LineRenderer>();
            lineRenderer.enabled = false;
        }
    }


    void OnValidate()
    {
        bodies = FindObjectsOfType<SolarsystemBody>();
        if (usePhysicsTimeStep)
        {
            timeStep = Universe.timeSteps;
        }
    }

    // ----- CLASS: to simulate and precalculate celestial bodies -------
    class SimulatedBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public SimulatedBody(SolarsystemBody body)
        {
            position = body.transform.position;
            velocity = body.startVelocity;
            mass = body.mass;
        }
    }
}
