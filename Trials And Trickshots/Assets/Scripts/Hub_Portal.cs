using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Hub_Portal : MonoBehaviour
{
    [SerializeField] private GameObject _portal;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private string _level;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == null) return;
        if(other.gameObject.tag == "disc" || other.gameObject.tag == "Player")
        {
            //break particales
                //BreakParticles();
            //activate teleport
            SceneManager.LoadScene(_level);
        }
    }

    private void BreakParticles()
    {
        GameObject firework = Instantiate(this.gameObject, this.transform.position, Quaternion.identity);

        firework.GetComponent<ParticleSystem>().Play();
    }
}
