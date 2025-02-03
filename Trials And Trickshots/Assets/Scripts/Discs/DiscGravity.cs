/******************************************************************************
 * Author: Brad Dixon
 * Creation Date: 2/2/2025
 * File Name: DiscGravity.cs
 * Brief: Controls the gravity that is applied to the disc
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscGravity : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [Tooltip("The gravity enacted on the disc")]
    [SerializeField] private float discGravity;

    /// <summary>
    /// Controls gravity on the disc
    /// </summary>
    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * (rb.mass * discGravity));
    }
}
