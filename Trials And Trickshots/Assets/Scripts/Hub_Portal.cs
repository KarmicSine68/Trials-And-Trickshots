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
    private void Start()
    {
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
            //break particales
                //BreakParticles(); //disabled for now
            //activate teleport
            SceneManager.LoadScene(_level);//sends them to level set in _level string in inspector.
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
