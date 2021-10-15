using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //Singleton
    [Header("Set in Inspector")]
    public float minDist = 0.1f;
    [Header("Set Dynamically")]
    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    void Awake()
    {
        S = this; // Set the singleton
        line = GetComponent<LineRenderer>(); // Get a reference to the LineRenderer
        // Disable the LineRenderer until if it's needed
        line.enabled = false;
        // Initialize the points List
        points = new List<Vector3>();
    }
    // This property (that is,a method masquerading as a field)
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                // When _poi is set to something new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    // This can be used ti clead the line directly
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }
    public void AddPoint()
    {
        // This is called to add a point to the line
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //If the point isn't far enouth from the last point, it returns
            return;
        }
        if (points.Count == 0)
        { // If this is launch point
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // To be defined
                                                               // it adds an extra bit of line to aid aiming later
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // Sets the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            // Enable the LineRenderer
            line.enabled = true;
        }
        else
        {
            // Normal behavior of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }
    // Returns the location of the most recently added point
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                // If there are no points, returns Vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }
    void FixedUpdate()
    {
        if (poi == null)
        {
            // If there is no poi, serch for one
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; // Return if we didn't find a poi
                }
            }
            else
            {
                return; // Return if we didn't fimd a poi
            }

        }
        // If the object of interest is found, try to add a point with its coordinates in each FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
        {
            // If FollowCam.POI contains null, write null to poi
            poi = null;
        }
    }
}