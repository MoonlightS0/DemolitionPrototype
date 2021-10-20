using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TreeCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numTrees = 5;  // Number of Tree
    public GameObject treePrefab;  // Template for Trees
    public Vector3 treePosMin = new Vector3(1, 0, 0);
    public Vector3 treePosMax = new Vector3(100, 0, 0);
    public float treeScaleMin = 1; // Min. scale of each Tree
    public float treeScaleMax = 2; // Max, scale of each Tree
    [Header("Set Dynamically")]


    private GameObject[] treeInstances;
    void Awake()
    {
        // Create an array to store all tree instances
        treeInstances = new GameObject[numTrees];
        // Find the Parent tree Anchor Game Object
        GameObject anchor = GameObject.Find("TreeAnchor");
        // Create a specified number of trees in a cycle
        GameObject tree;
        for (int i = 0; i < numTrees; i++)
        {
            // Create an instance of treePrefab
            tree = Instantiate<GameObject>(treePrefab);
            // Choose a location for the tree
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(treePosMin.x, treePosMax.x);
            cPos.y = Random.Range(treePosMin.y, treePosMax.y);
            // Scale the tree
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(treeScaleMin, treeScaleMax, scaleU);
            //Smaller tree (with a smaller scale value) should be closer to the ground
            cPos.y = Mathf.Lerp(treePosMin.y, cPos.y, scaleU); //if the scale of the object is large then the y positioning will be larger
            // Smaller Trees should be further away
            cPos.z = 100 - 90 * scaleU;
            // Apply the obtained coordinate and scale values to the tree
            tree.transform.position = cPos;
            tree.transform.localScale = Vector3.one * scaleVal;
            // Make the tree a child of anchor
            tree.transform.SetParent(anchor.transform);
            // Add a tree to the Tree Instances array
            treeInstances[i] = tree;
        }
    }
    void Update()
    {

    }
}
