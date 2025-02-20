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
    private CinemachineConfiner sceneConfiner;
    private GameObject boundingVolume;

    void Start()
    {
        sceneConfiner = GetComponentInChildren<CinemachineConfiner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Confiner"))
        {
            boundingVolume = other.gameObject;
            if (boundingVolume != null)
            {
                sceneConfiner.m_BoundingVolume = boundingVolume.GetComponent<Collider>();
                Debug.Log("Bounding volume changed to: " + boundingVolume.name);
            }
            else
            {
                Debug.LogError("Bounding volume is null!");
            }
        }
    }
}
