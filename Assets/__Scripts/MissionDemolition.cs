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
    public static Text          scoreGT;


    void Start()
    {
        // Get a link to the ScoreCounter game object
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        // Get the Text component of this game object
        scoreGT = scoreGO.GetComponent<Text>();
        //Set the initial number of points to 0
        scoreGT.text = "0";

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

            // Add points for the hit at green zone
            int numCh = 3;
            AccessingToCurrentScoreOfPlayer(numCh);
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

        //Minus one score for one shot
        int numCh = -1;
        AccessingToCurrentScoreOfPlayer(numCh);
        
    }

    static void AccessingToCurrentScoreOfPlayer(int numCh)//numCh - number change of score,also it remeber high score
    {
        // Convert text to scoreGT to an integer
        int score = int.Parse(scoreGT.text);
        //Changing score
        score += numCh;
        // Convert the number of points back to a string and display it on the screen
        scoreGT.text = score.ToString();
        //Remember the highest achievement
        if (score > HighScore.score)
        {
            HighScore.score = score;
        }
    }

}