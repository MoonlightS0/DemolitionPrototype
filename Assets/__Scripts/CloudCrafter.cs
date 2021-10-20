using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40;  // Number of clouds
    public GameObject cloudPrefab;  // Template for clouds
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1; // Min. scale of each cloud
    public float cloudScaleMax = 3; // Max, scale of each cloud
    public float cloudSpeedMult = 0.5f; // Cloud velocity coefficient
    

   [Header("Set Dynamically")]


    private GameObject[] cloudInstances;
    void Awake()
    {
        // Create an array to store all cloud instances
        cloudInstances = new GameObject[numClouds];
        // Find the Parent Cloud Anchor Game Object
        GameObject anchor = GameObject.Find("CloudAnchor");
        // Create a specified number of clouds in a cycle
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            // Create an instance of cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            // Choose a location for the cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            // Scale the cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //Smaller clouds (with a smaller scale value) should be closer to the ground
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU); //if the scale of the object is large then the y positioning will be larger
            // Smaller clouds should be further away
            cPos.z = 100 - 90 * scaleU;
            // Apply the obtained coordinate and scale values to the cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            // Make the cloud a child of anchor
            cloud.transform.SetParent(anchor.transform);
            // Add a cloud to the Cloud Instances array
            cloudInstances[i] = cloud;
        }
    }
    void Update()
    {
        // Bypass all created clouds in a loop
        foreach (GameObject cloud in cloudInstances)
        {
            // Get the scale and coordinates of the cloud
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            // Increase speed for near clouds
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            // If the cloud has shifted too far to the left
            if (cPos.x <= cloudPosMin.x)
            {
                // Move it far to the right
                cPos.x = cloudPosMax.x;
            }
            // Apply new coordinates to the cloud
            cloud.transform.position = cPos;
        }
    }
}
