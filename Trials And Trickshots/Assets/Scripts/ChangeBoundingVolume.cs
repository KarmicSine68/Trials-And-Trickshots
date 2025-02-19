/******************************************************************************
 * Author: Skylar Turner
 * Creation Date: 2/18/2025
 * File Name: ChangeBoundingVolume.cs
 * Brief: Makes sure the Cinemachine confiner is set to the correct bounding volume
 * ***************************************************************************/
using UnityEngine;
using Cinemachine;

public class ChangeBoundingVolume : MonoBehaviour
{
    CinemachineConfiner sceneConfiner;
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
