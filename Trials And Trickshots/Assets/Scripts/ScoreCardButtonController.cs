using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreCardButtonController : MonoBehaviour
{
    private ScoreManager sm;
    private void Start()
    {
        sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }
    public void ReturnToHubClicked()
    {
        SceneManager.LoadScene("Hub");
    }
    public void ReplayLevelClicked()
    {
        sm.LogThrows(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
