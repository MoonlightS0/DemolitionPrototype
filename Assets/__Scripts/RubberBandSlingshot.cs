using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBandSlingshot : MonoBehaviour
{
    [Header("Set in Inspector")]
    
    [Header("Set Dynamically")]
    private LineRenderer rubberBandSLine;
    private Vector3 pointsRubberBand;
    public Slingshot aimM;

    void Awake()
    {
        rubberBandSLine = GetComponent<LineRenderer>();
        // Disable the LineRenderer until if it's needed
        rubberBandSLine.enabled = false;
    }

    public void Clear()
    {
        rubberBandSLine.enabled = false;
    }

    void FixedUpdate()
    {
        //Take and transform mouse pos for draw line(Band)
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);    //transform coordinates of mouse in global
        Vector3 mouseDelta = mousePos3D - Slingshot.LAUNCH_POS;    // Find the coordinate difference between Slingshot.LAUNCH_POS and mousePos3D
        float maxMagnitude = Slingshot.S.GetComponent<SphereCollider>().radius;     //Limit mouseDelta to the radius of the collider of the Slingshot object
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        Vector3 DifferenceOfCenterS = new Vector3(-10,-6,0); //This vector is necessary for correct calculations of the central slingshot
        pointsRubberBand = mouseDelta + DifferenceOfCenterS;
        

        if (aimM.aimingMode == true)    //With this chek line draw only if player in aiming mode.(*1)
        {
            //length of the Rubber Band are limited
            float dist = Vector3.Distance(Slingshot.LAUNCH_POS, pointsRubberBand);
            if (dist > 15)
            {
                Clear();
                return;
            }
            //If length is normal - line draw.
            else
            {
                rubberBandSLine.SetPosition(0, pointsRubberBand);
                rubberBandSLine.SetPosition(1, Slingshot.LAUNCH_POS);
                rubberBandSLine.enabled = true;
                return;
            }

        }

        else
        //clear the line and return.
        {
            Clear();
            return;
        }
    }
}
