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

public class ScoreManager : MonoBehaviour
{
    private int currentThrows;
    private List<int> scores;
    public Text ThrowText;

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
    public int GetThrowsFromHole(int holeNumber) {
        if (holeNumber > scores.Count) {
            return scores[scores.Count];
        }
        if (holeNumber < 1){
            return scores[0];
        }
        return scores[holeNumber - 1];
    }

    //sets the count of throws to 0
    public void ResetThrows() {
        currentThrows = 0;
        SetScoreText();
    }

    // logs the # of throws on for the hole you finished and then resets the count
    public void LogThrows() {
        scores.Add(currentThrows);
        ResetThrows();
    }

    //sets the throws Text to the current throws;
    private void SetScoreText() {
        ThrowText.text = "Throws: " + currentThrows;
    }
}
