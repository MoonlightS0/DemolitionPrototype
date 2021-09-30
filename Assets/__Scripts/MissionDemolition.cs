using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // The hidden object is a singleton
    [Header("Set in Inspector")]
    public Text          uitLevel;    // Reference to the UIText_Level object
    public Text          uitShots;// Reference to the UIText_Shots object
    public Text          uitButton;// Reference to the child Text object in UIButton_View
    public Vector3       castlePos;// Location of castle
    public GameObject[]  castles;// Array of castles

    [Header("Set Dynamically")]
    public int           level; // Current level
    public int           levelMax; // Number of levels
    public int           shotsTaken;
    public GameObject    castle; // Current castle
    public GameMode      mode = GameMode.idle;
    public string        showing = "Show Slingshot"; // FollowCam mode

    void Start()
    {
        S = this; // Define a single object

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel()
    {
        // Destroy the old castle, if it exists
        if (castle != null) {
            Destroy(castle);
        }

        // Destroy the previous projectiles, if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        // Create a new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // Reset the camera to the starting position
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        // Reset the target
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        // Show data in UI elements
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();
        // Check level completion
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            // Change the mode; to stop checking the completion of the level
            mode = GameMode.levelEnd;
            // Zoom out
            SwitchView("Show Both");
            // Start a new level in 2 seconds
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel() {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

public void SwitchView(string eView = "")
{
    if (eView == "")
    {
        eView = uitButton.text;
    }
    showing = eView;
    switch (showing)
    {
        case "Show Slingshot":
        FollowCam.POI = null;
        uitButton.text = "Show Castle";
        break;

        case "Show Castle":
        FollowCam.POI = S.castle;
        uitButton.text = "Show Both";
        break;

        case "Show Both":
        FollowCam.POI = GameObject.Find("ViewBoth");
        uitButton.text = "Show Slingshot";
        break;
    }
}
    // A static method that allows you to increase shotsTaken from any code
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}