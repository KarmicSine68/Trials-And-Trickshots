/******************************************************************************
 * Author: Tyler Bouchard
 * Creation Date: 1/30/2025
 * File Name: ScoreManager.cs
 * Brief: manipulates and keeps track of the score for the player
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    private int currentThrows;
    public Text throwText;
    public Text hole1ScoreText;
    public Text hole2ScoreText;
    public Text totalScoreText;
    public GameObject scoreCard;

    private void Start()
    {
        ResetThrows();
    }
    //adds one to score
    public void IncreaseThrows()
    {
        currentThrows++;
        SetScoreText();
    }

    //overlaod for IncreaseScore that increases the score by a specific amount
    //idk if this is needed but dosent hurt to have
    public void IncreaseThrows(int amount) {
        currentThrows += amount;
        SetScoreText();
    }

    //returns the current amount of throws
    public int GetCurrentThrows() {
        return currentThrows;
    }

    //returns the score that was logged from a certian hole / level
    public int GetThrowsFromHole(string hole) {
        if (PlayerPrefs.HasKey(hole)){
            return PlayerPrefs.GetInt(hole);
        } else {
            Debug.Log("Hole does not exist");
            return 0;
        }
    }

    //sets the count of throws to 0
    public void ResetThrows() {
        currentThrows = 0;
        SetScoreText();
    }
    //resets the playerprefs to 0
    public void ResetHighScores()
    {
        PlayerPrefs.SetInt("lvOne", 0);
        PlayerPrefs.SetInt("levelThree", 0);
    }
    // logs the # of throws on for the hole you finished and then resets the count
    public void LogThrows(string hole) {
        PlayerPrefs.SetInt(hole, currentThrows);
    }
    public void ShowScoreCard() {
        scoreCard.SetActive(true);
        hole1ScoreText.text = "" + PlayerPrefs.GetInt("LvOne");
        hole2ScoreText.text = "" + PlayerPrefs.GetInt("LevelThree");
        totalScoreText.text = "" + (PlayerPrefs.GetInt("LvOne") + PlayerPrefs.GetInt("LevelThree"));
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    //sets the throws Text to the current throws;
    private void SetScoreText() {
        throwText.text = "Throws: " + currentThrows;
    }
    
}
