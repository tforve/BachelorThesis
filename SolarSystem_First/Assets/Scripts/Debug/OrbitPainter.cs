using System;
using UnityEngine;

/*  Simulate Bodies with extra SimulatedBody Class
    *   Calculate their flightpath
    *   Draw flightpath
    *   also add features to turn on or off the drawen Path
    *   maybe show flithpath in total, and also in realtime.
    */
[ExecuteInEditMode]
public class OrbitPainter : MonoBehaviour
{
    #region SINGLETON PATTERN
    private static OrbitPainter _instance;
    public static OrbitPainter Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<OrbitPainter>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("DebugOrbit");
                    _instance = container.AddComponent<OrbitPainter>();
                }
            }


            return _instance;
        }

    }
    #endregion

    public int numSteps = 1170;                 // precalculated Steps
    public float timeStep = 0.1f;

    [Header("Centralody")]
    public SolarsystemBody centralBody;         // reference body for center of system
    public bool relativeToCentralBody;          // if true, reference to centralBody
    [Header("Drawing")]
    public bool drawInPlayMode = false;
    public float thickness = 20;                // width of linerenderer

    [Header("Celectial Bodies")]
    private SolarsystemBody[] bodies;           // actual bodies of system
    private SimulatedBody[] simulatedBodies;    // placeholder to calculate futur Positions
    private Vector3[][] drawPoints;             // points inbetween we draw lines
    private int referenceFrameIndex = 0;
    private Vector3 referenceBodyInitialPosition = Vector3.zero;

    public bool addBodiesToDebug = false;       // used to check if new bodies were added

    void Start()
    {
        bodies = FindObjectsOfType<SolarsystemBody>();
    }

    void Update()
    {
        UpdateOrbit();

        if (addBodiesToDebug)
        {
            bodies = FindObjectsOfType<SolarsystemBody>();
            addBodiesToDebug = false;
        }
    }

    void UpdateOrbit()
    {
        if (drawInPlayMode || !Application.isPlaying)
        {
            InitializeSimulatedBodies();
            SimulateFlightPath();
            DrawFlightPath();
        }

    }

    void InitializeSimulatedBodies()
    {
        simulatedBodies = new SimulatedBody[bodies.Length];
        drawPoints = new Vector3[bodies.Length][];

        for (int i = 0; i < simulatedBodies.Length; i++)
        {
            simulatedBodies[i] = new SimulatedBody(bodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            // save Index and position of centralBody to reference
            if (bodies[i] == centralBody && relativeToCentralBody)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = simulatedBodies[i].position;
            }
        }
    }

    /// <summary>
    /// Calculate the flightpath for each SimulatedBody
    /// </summary>
    private void SimulateFlightPath()
    {
        // Simulate
        for (int step = 0; step < numSteps; step++)
        {
            // celectial body which is seen as the center / set referenceBodyPos to simulatedBody at Position of centralBody
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
    }

    private void DrawFlightPath()
    {
        for (int i = 0; i < simulatedBodies.Length; i++)
        {
            Material pathMaterial = bodies[i].gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            Color pathColor = pathMaterial.color;
            LineRenderer lineRenderer = bodies[i].gameObject.GetComponentInChildren<LineRenderer>();

            if (drawInPlayMode)
            {
                lineRenderer.enabled = true;
                lineRenderer.positionCount = drawPoints[i].Length;
                lineRenderer.SetPositions(drawPoints[i]);
                lineRenderer.material = pathMaterial;
                lineRenderer.widthMultiplier = thickness;
            }
            else
            {
                for (int j = 0; j < drawPoints[i].Length - 1; j++)
                {
                    Debug.DrawLine(drawPoints[i][j], drawPoints[i][j + 1], pathColor);
                }

                lineRenderer.enabled = false;
            }
        }
    }

    /// <summary>
    /// Calculate Acceleration of all celectial bodies
    /// </summary>
    private Vector3 CalculateAcceleration(int i, SimulatedBody[] simulatedBodies)
    {
        Vector3 acceleration = Vector3.zero;

        for (int j = 0; j < simulatedBodies.Length; j++)
        {
            if (i == j)
            {
                continue;
            }
            // dir vector to each other
            Vector3 direction = (simulatedBodies[j].position - simulatedBodies[i].position).normalized;
            //calculate distance to each other | r^2 
            float sqrDistance = (simulatedBodies[j].position - simulatedBodies[i].position).sqrMagnitude;
            // force (F = G *((m1*m2)/r^2))
            Vector3 force = direction * Universe.gravitationalConstant * ((simulatedBodies[j].mass * simulatedBodies[i].mass) / sqrDistance);
            acceleration += force / simulatedBodies[i].mass;
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
    }

    private void ResetOrbit()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            LineRenderer lineRenderer = bodies[i].gameObject.GetComponentInChildren<LineRenderer>();
            lineRenderer.enabled = false;
        }
    }

    // ----- CLASS: to simulate and precalculate celestial bodies -------
    private class SimulatedBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public SimulatedBody(SolarsystemBody systemBody)
        {
            this.position = systemBody.transform.position;
            this.velocity = systemBody.currentVelocity;
            this.mass = systemBody.mass;
        }
    }



}
