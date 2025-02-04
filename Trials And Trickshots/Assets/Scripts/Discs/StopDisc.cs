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
    [SerializeField] private Rigidbody rb;

    [Header("Shield variables")]
    [Tooltip("While active, the disc will bounce off the ground instead of stopping")]
    [SerializeField] private bool shielded;

    [Tooltip("How many times the shield can hit the ground before it breaks")]
    [SerializeField] private int shieldHits;

    [Tooltip("How many seconds it has before the shield can lose another hit")]
    [SerializeField] private float shieldInvulnerabilityTime;

    [Tooltip("How much the disc will bounce off the ground while shielded")]
    [SerializeField] private float upwardsBounce;

    [Header("Box Cast variables")]
    [SerializeField] private LayerMask ground;
    [SerializeField] private Collider discBox;

    private bool grounded = false;
    private bool canHitShield = true;

    /// <summary>
    /// Checks for when the disc hits the ground
    /// </summary>
    private void FixedUpdate()
    {

        //Checks that the disc has hit the ground for the first time while not shielded.
        //Also makes sure the shield is able to be hit again
        if(HitGround() && !grounded && canHitShield)
        {
            //If no shield, enable friction to slow the disc down
            if(!shielded)
            {
                grounded = true;
                GetComponent<Collider>().material.dynamicFriction = 2;
                GetComponent<Collider>().material.bounciness = 0;
            }
            else
            {
                canHitShield = false;

                StartCoroutine(EnableShieldDamage());

                --shieldHits;
                Debug.Log(shieldHits);

                //Disables shield if it runs out of durability
                if (shieldHits <= 0)
                {
                    shielded = false;
                }

                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * upwardsBounce, ForceMode.Impulse);
            }
        }
    }

    /// <summary>
    /// Waits x amount of time before letting the shield take damage again.
    /// Done so the shield doesn't take damage multiple times
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableShieldDamage()
    {
        float i = shieldInvulnerabilityTime;
        while (i >= 0)
        {
            yield return new WaitForSeconds(.1f);

            i -= .1f;
        }

        canHitShield = true;
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
