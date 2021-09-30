using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
        static public GameObject POI;   // Link to the object of interest

        [Header("Set in Inspector")]    
        public float easing = 0.05f;    //The value of the interpolation - now it is ~5%
        public Vector2 minXY = Vector2.zero;    //The value for the correct function - follow camera.Value=[0,0].Limitations negatives values of camera.
        [Header("Set Dynamically")] 
        public float camZ;  // Desired camera Z coordinate
        void Awake()
        {
            camZ = this.transform.position.z;
        }
        void FixedUpdate()
        {
        //if (POI == null) return;    // Exit if there is no object of interest
        //Vector3 destination = POI.transform.position;   // Get the position of the object of interest
        Vector3 destination;
        // If there is no object of interest, return P:[ 0, 0, 0 ]
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            // Get the position of the object of interest
            destination = POI.transform.position;
            // If the object of interest is a projectile, make sure that it has stopped
            if (POI.tag == "Projectile")
            {
                //If he is standing still (that is, not moving)
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    // Return to the original settings of the camera's field of view
                    POI = null;
                    //in the next frame
                    return;
                }
            }
        }
        destination.x = Mathf.Max(minXY.x, destination.x);  // Limit X and Y to the minimum values
        destination.y = Mathf.Max(minXY.y, destination.y);  
        destination = Vector3.Lerp(transform.position, destination, easing);// Determine the point between the current camera location and "destination"
        // Force the destination value to be set.z is equal to camZ, so that
        // Move the camera away
        destination.z = camZ;
        transform.position = destination;    // Put the camera in the destination position
        Camera.main.orthographicSize = destination.y + 10;   //Change the size of the orthographicSize of the camera.So that the earth remains in the field of view

    }
}