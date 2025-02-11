using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Hub_Portal : MonoBehaviour
{
    [SerializeField] private GameObject _portal;//reference to the physical portal
    [SerializeField] private ParticleSystem _particleSystem;//when the disc/player 
    [SerializeField] private string _level;//enter level name.
    [SerializeField] private bool portalVisible;
    //private ScoreManager sm;
    private void Start()
    {
        //sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        if(!portalVisible)
        {
            _portal.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == null) return;
        if(other.gameObject.GetComponent("Disc") || other.gameObject.GetComponent("PlayerBehaviour")) //checks to see if the player walks into the portal or the disc makes contact
        {
            SceneManager.LoadScene(_level);
            //break particales
            //BreakParticles(); //disabled for now
            //activate teleport
            //if (_level == "Hub")
            //{
            //    SceneManager.LoadScene(_level);
            //}
            //else {
            //    //sm.LogThrows(SceneManager.GetActiveScene().name);
            //    //sm.ShowScoreCard();
            //}
            //sends them to level set in _level string in inspector.

        }
    }

    private void BreakParticles()
    {
        GameObject firework = Instantiate(this.gameObject, this.transform.position, Quaternion.identity);//creates particle 

        firework.GetComponent<ParticleSystem>().Play();//plays particle for when the player/disc touches the portal 
    }

    private void PortalAppear()
    {
        if(!portalVisible)
        {
            _portal.SetActive(true);
        }
    }
}
