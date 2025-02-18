using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeBoundingVolume : MonoBehaviour
{
    CinemachineConfiner sceneConfiner;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    GameObject boundingVolume;


    // Start is called before the first frame update
    void Start()
    {
        boundingVolume = GameObject.FindGameObjectWithTag("Confiner");
        sceneConfiner = GetComponent<CinemachineConfiner>();
        sceneConfiner.m_ConfineMode = CinemachineConfiner.Mode.Confine3D;
        sceneConfiner.m_BoundingVolume = boundingVolume.GetComponent<Collider>();
    }
}
