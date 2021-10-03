using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    static public int score = 1;      //static public required for read score and work with him.
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();

        //If the HighScore value already exists in PlayerPrefs, read it
        if (PlayerPrefs.HasKey("HighScore"))
        {
            score = PlayerPrefs.GetInt("HighScore");
        }
        //Save the highest achievement of HighScore to the repository in Unity
        PlayerPrefs.SetInt("HighScore", score);
        Debug.Log("New High Score writed!");
    }

    // Update is called once per frame
    void Update()
    {
        Text gt = this.GetComponent<Text>();
        gt.text = "High Score: " + score;
        //Update HighScore in PlayerPrefs, if necessary
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}