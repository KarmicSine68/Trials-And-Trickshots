/******************************************************************************
 * Author: Brad Dixon
 * Creation Date: 2/2/2025
 * File Name: StopDisc.cs
 * Brief: Stops the disc's movement if it touches the ground
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopDisc : MonoBehaviour
{
    [Header("Shield variables")]
    [Tooltip("While active, the disc will bounce off the ground instead of stopping")]
    [SerializeField] private bool shielded;

    [Tooltip("How many times the shield can hit the ground before it breaks")]
    [SerializeField] private int shieldHits;

    [Header("Box Cast variables")]
    [SerializeField] private LayerMask ground;
    [SerializeField] private Collider discBox;

    private bool grounded = false;

    /// <summary>
    /// Checks for when the disc hits the ground
    /// </summary>
    private void FixedUpdate()
    {
        //if (HitGround() &&!grounded)
        //{
        //    if (shielded)
        //    {
        //        --shieldHits;
        //        Debug.Log(shieldHits);

        //        if (shieldHits <= 0)
        //        {
        //            shielded = false;
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("Ground hit");
        //        grounded = true;

        //        StopMovement();
        //    }
        //}

        if(GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            Debug.Log("Disc stopped");
        }

        if(HitGround() && !grounded)
        {
            if(!shielded)
            {
                grounded = true;
                GetComponent<Collider>().material.dynamicFriction = 1;
            }
            else
            {
                --shieldHits;
                Debug.Log(shieldHits);

                if (shieldHits <= 0)
                {
                    shielded = false;
                }
            }
        }
    }

    /// <summary>
    /// Returns true if the disc hits the ground
    /// </summary>
    /// <returns></returns>
    private bool HitGround()
    {
        return Physics.BoxCast(discBox.bounds.center, discBox.bounds.size, Vector3.down, Quaternion.identity, .01f);
    }
}
