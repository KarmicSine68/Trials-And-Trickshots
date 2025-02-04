using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalBackToHub : MonoBehaviour
{
    [SerializeField] private string _level;//enter level name.

    private void OnTriggerEnter(Collider other)
    {
        if(other == null) return;
        if(other.gameObject.GetComponent("DiscGravity") || other.gameObject.GetComponent("PlayerBehaviour")) //checks to see if the player walks into the portal or the disc makes contact
        {
            //break particales
                //BreakParticles(); //disabled for now
            //activate teleport
            SceneManager.LoadScene(_level);//sends them to level set in _level string in inspector.
        }
    }
}
