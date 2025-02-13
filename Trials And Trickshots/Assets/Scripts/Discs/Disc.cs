/******************************************************************************
 * Author: Brad Dixon
 * Creation Date: 2/2/2025
 * File Name: Disc.cs
 * Updates: The gravity code was written by Skylar Turner
 * Brief: Controls the disc's gravity, teleportation, and stopping
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;

    [Tooltip("The friction applied to the disc when it touches the ground")]
    [SerializeField] protected float discFriction;

    [Tooltip("The gravity enacted on the disc")]
    [SerializeField] protected float discGravity;

    [Tooltip("How long the delay is to teleport the player after the disc stops")]
    private float delayTime;

    [Header("Shield variables")]
    [Tooltip("While active, the disc will bounce off the ground instead of stopping")]
    [SerializeField] private bool shielded;

    [Tooltip("How many times the shield can hit the ground before it breaks")]
    [SerializeField] private int shieldHits;

    [Tooltip("How many seconds it has before the shield can lose another hit")]
    [SerializeField] private float shieldInvulnerabilityTime;

    [Tooltip("How much the disc will bounce off the ground while shielded")]
    [SerializeField] private float upwardsBounce;

    [Header("")]
    [SerializeField] protected Collider discBox;

    protected bool grounded = false;
    private bool canHitShield = true;

    //Teleportation variables
    protected Vector3 landingPosition;
    protected GameObject player;

    protected bool stopped = false;

    /// <summary>
    /// Gets a reference to the player gameobject
    /// </summary>
    protected void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    /// <summary>
    /// Calls the code for ground collision and gravity
    /// </summary>
    protected void FixedUpdate()
    {
        GroundCollision();

        DiscGravity();

        if(grounded)
        {
            Debug.Log(rb.velocity);
            if (Mathf.Abs(rb.velocity.x) <= .001 && Mathf.Abs(rb.velocity.z) <= .001
                && !stopped)
            {
                stopped = true;

                rb.velocity = Vector3.zero;

                landingPosition = transform.position;

                StartCoroutine(DelayTeleport());
            }
        }
    }

    /// <summary>
    /// Applies a downward force on the disc
    /// </summary>
    protected virtual void DiscGravity()
    {
        rb.AddForce(Vector3.down * (rb.mass * discGravity));
    }

    /// <summary>
    /// Either enacts friction on the disc or updates the disc's shield when the
    /// disc hits the ground
    /// </summary>
    protected void GroundCollision()
    {
        //Checks that the disc has hit the ground for the first time while not shielded.
        //Also makes sure the shield is able to be hit again
        if (HitGround() && !grounded && canHitShield)
        {
            //If no shield, enable friction to slow the disc down
            if (!shielded)
            {
                grounded = true;
                GetComponent<Collider>().material.dynamicFriction = discFriction;
                GetComponent<Collider>().material.bounciness = 0;
            }
            //Decreases shield durability
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
    protected virtual bool HitGround()
    {
        return Physics.BoxCast(discBox.bounds.center, discBox.bounds.size, Vector3.down, Quaternion.identity, .03f);
    }

    /// <summary>
    /// Changes the players transform position to be where the landing position
    /// of the disc was.
    /// </summary>
    protected void TeleportPlayer()
    {
        if (player != null)
        {
            player.transform.position = landingPosition;

            DiscThrower dt = FindObjectOfType<DiscThrower>();
            dt.ReadyDisc();
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Waits x amount of timne after the disc stops before teleporting the player
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DelayTeleport()
    {
        yield return new WaitForSeconds(delayTime);

        TeleportPlayer();
    }
}
