using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class LevelWaypointGenerator : MonoBehaviour
{
    public static LevelWaypointGenerator Instance;
    public bool CreatePath = true;

    [Range(2, 99)]
    [SerializeField] private int waypoints = 8;
    [SerializeField] private float waypointDistance = 10;
    [SerializeField] private float waypointAngleMin = 5;
    [SerializeField] private float waypointAngleMax = 20;

    public GeneratePathExample pathWaypointGenerator;
    [HideInInspector] public PathCreator pathCreator;
    [HideInInspector] public float pathLength;
    [HideInInspector] public PathPlacer pathPlacer;

    void Awake()
    {
        Instance = this;
        if (pathWaypointGenerator == null) Debug.LogWarning("GeneratePathExample not set!"); //= this.gameObject.GetComponent<GeneratePathExample>();
        pathCreator = pathWaypointGenerator.gameObject.GetComponent<PathCreator>();
        pathPlacer = pathWaypointGenerator.gameObject.GetComponent<PathPlacer>();

        if (CreatePath)
        {
            Debug.Log("Building Path Geometry");
            //DestroyObjects();
            BuildWaypoints();
            pathWaypointGenerator.CreateBezier();
            BuildPathGeometry();
            pathLength = pathCreator.path.length;
        }
        if (pathCreator == null) Debug.LogWarning("NO pathCreator!");
    }
    private void Start()
    {
        //StartCoroutine(BatchPathGeometry()); // Doesn't work
    }

    public void BuildWaypoints()
    {
        //Debug.Log("Building Waypoints");

        Transform[] waypointTransforms = new Transform[waypoints];
        GameObject previousPoint = this.gameObject;
        string objName = "Waypoint";

        pathPlacer.DestroyObjects();

        for (int i = 0; i < waypoints; i++)
        {
            GameObject point = new GameObject(objName+i);
            point.transform.position = previousPoint.transform.position;
            point.transform.rotation = previousPoint.transform.rotation;

            int xFlip = 1; int yFlip = 1;
            if (i > 0)
            {
                if (Random.value < 0.5f) xFlip = -1;
                if (Random.value < 0.5f) yFlip = -1;

                if (i > 1) point.transform.eulerAngles = new Vector3(
                    previousPoint.transform.rotation.x + Random.Range(waypointAngleMin, waypointAngleMax) * xFlip,
                    previousPoint.transform.rotation.y + Random.Range(waypointAngleMin, waypointAngleMax) * yFlip);
                point.transform.position += point.transform.forward * waypointDistance;
            }

            waypointTransforms[i] = point.transform;

            point.transform.parent = this.gameObject.transform;
            previousPoint = point;
        }
        pathWaypointGenerator.waypoints = waypointTransforms;
    }

    public void BuildPathGeometry()
    {
        //Debug.Log("Building Path Geometry");
        pathPlacer.Generate();
    }

    public void DestroyObjects()
    {
        int numChildren = pathWaypointGenerator.gameObject.transform.childCount;
        for (int i = numChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(pathWaypointGenerator.gameObject.transform.GetChild(i).gameObject, false);
        }
    }

    /// <summary>
    /// Attempt to initiatiate patching outside of Start -functions
    /// </summary>
    /// <returns></returns>
    IEnumerator BatchPathGeometry()
    {
        //Debug.Log("Start coroutine");
        //yield return null;
        yield return new WaitForSeconds(1f);
        //Debug.Log("Start batching");
        StaticBatchingUtility.Combine(this.gameObject);
    }
}

