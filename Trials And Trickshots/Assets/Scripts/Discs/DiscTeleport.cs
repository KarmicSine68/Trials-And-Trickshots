/******************************************************************************
 * Author: Skylar Turner
 * Creation Date: 2/3/2025
 * File Name: DiscTeleport.cs
 * Brief: Teleports the player to the point where the disc touches the ground
 * ***************************************************************************/
using UnityEngine;

public class DiscTeleport : MonoBehaviour
{
    private Vector3 landingPosition;
    private GameObject player;

    /// <summary>
    /// Gets a reference to the player gameobject
    /// </summary>
    private void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().gameObject;    
    }

    /// <summary>
    /// If the collision object has the tag "Ground" it will get the point at
    /// which it touches the ground and saves it, destroys the disc object, and
    /// calls the TeleportPlayer() function.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            landingPosition = transform.position;
            Destroy(gameObject);
            TeleportPlayer();
        }
    }

    /// <summary>
    /// Changes the players transform position to be where the landing position
    /// of the disc was.
    /// </summary>
    private void TeleportPlayer()
    {
        if (player != null)
        {
            player.transform.position = landingPosition;
        }
    }
}
