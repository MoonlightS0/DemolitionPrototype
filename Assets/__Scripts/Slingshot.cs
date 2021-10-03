using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S; //This is a hidden static instance of Slingshot, which will play the role of a single object
    // fields that are set in the Unity inspector
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;
    // fields that are set in the Unity inspector dinamicly
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }

    void Awake()
    {
        S = this;   //The S field will be set before some other code tries to access the LAUNCH_POSITION property.
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }


    void OnMouseEnter()     //function for detecting(in console) the player's mouse in a close radius
                            //also that turn on the light (zone of control of the slingshot) - detection for user
    {
        print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }
    void OnMouseExit()      //function for detecting(in console) the exit of the player's mouse from the radius
                            //also that turn off the light (zone of control of the slingshot) - detection for user
    {
        print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }
   
    void OnMouseDown()
    {

        aimingMode = true;  // The player pressed the mouse button when the pointer was over the slingshot
        projectile = Instantiate(prefabProjectile) as GameObject;   // Create a projectile
        projectile.transform.position = launchPos;  // Put it in a launchPoint
        // Make it kinematic
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;

    }
    void Update()
    {
        
        if (!aimingMode) return;    // If the slingshot is not in aiming mode, do not execute this code
        Vector3 mousePos2D = Input.mousePosition; 
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);    //transform coordinates of mouse in global
        Vector3 mouseDelta = mousePos3D - launchPos;    // Find the coordinate difference between launchPos and mousePos3D - direction of projectile
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;    // Limit mouseDelta to the radius of the collider of the Slingshot object
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        // Move the projectile to a new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;// The mouse button is released
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile; //write field-link for camera.Value to FollowCam class.
            projectile = null;  //clear the field-link for another projectile
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;

        }
    }
}