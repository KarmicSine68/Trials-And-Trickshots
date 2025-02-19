using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clicked : MonoBehaviour
{
    [SerializeField] private string _level;    // Start is called before the first frame update


    public void Activate()
    {
        SceneManager.LoadScene(_level);
    }
}
