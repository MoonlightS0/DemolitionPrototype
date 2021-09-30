using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")] 
    public GameObject cloudSphere;
    public int numSpheresMin = 6;
    public int numSpheresMax = 10;
    public Vector3 sphereOffsetScale = new Vector3(5,2,1); // Distance from center
    public Vector2 sphereScaleRangeX = new Vector2(4,8);// Scale range for each axis,the width of which is 2 times greater than high
    public Vector2 sphereScaleRangeY = new Vector2(3,4);
    public Vector2 sphereScaleRangeZ = new Vector2(2,4);
    public float scaleYMin = 2f;

    private List<GameObject> spheres;//List of spheres

    void Start()
    {
        spheres = new List <GameObject> ();

        int num = Random.Range(numSpheresMin, numSpheresMax);
        for (int i = 0; i < num; i++)
        {
            GameObject sp = Instantiate<GameObject>(cloudSphere);   // New copy-sphere
            spheres.Add(sp); // Add list of spheres
            Transform spTrans = sp.transform;   // Property "transform" of every "CloudSphere" transmitted to "spTrans" 
            spTrans.SetParent(this.transform);  //And every parent of sphere is "Transform"

            // Pick a random location
            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            spTrans.localPosition = offset;

            // Pick a random scale
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);  //x-min,y-max value of scale
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);
            //The scale changes along the Y axis, depending on the offset of Cloudsphere from the center of the
            //Cloud along the X axis. The farther the sphere is from the center of the cloud, the smaller the scale on the Y axis.
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x); 
            scale.y = Mathf.Max(scale.y, scaleYMin);
            spTrans.localScale = scale; //scale from parent
        }
    }
    // Update is called every frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))    //scan input key "space" for restart
    //    {
    //    Restart();
    //    }
    //}
    void Restart()      //destroy all child-objects spheres 
    {
        // Remove the old spheres that make up the cloud
        foreach (GameObject sp in spheres)
            {
            Destroy(sp);
            }
        Start();
    }
}