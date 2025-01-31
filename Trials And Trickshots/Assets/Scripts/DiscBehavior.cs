using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DiscBehavior : MonoBehaviour
{
    private Vector3 landingPosition;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            landingPosition = transform.position;
            Destroy(gameObject);
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            player.transform.position = landingPosition;
        }
    }
}
