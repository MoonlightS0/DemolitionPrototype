using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    // A static field accessible to any other code
    static public bool goalMet = false;
    void OnTriggerEnter(Collider other)
    {
        // When something falls into the scope of the trigger,
        // Check if this ”something" is a projectile
        if (other.gameObject.tag == "Projectile")
        {
            // If it is a projectile, set the goalMet field to true
            Goal.goalMet = true;
            // Also change the alpha channel of the color to increase opacity
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
