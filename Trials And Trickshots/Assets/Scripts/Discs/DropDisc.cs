/******************************************************************************
 * Author: Brad Dixon
 * File Name: DropDisc.cs
 * Creation Date: 2/4/2025
 * Brief: Code for a disc that drops immediatly to the ground
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropDisc : Disc
{
    InputActionMap actionMap;
    InputAction drop;

    [Tooltip("Higher values allow the drop disc to fall faster when it drops")]
    [SerializeField] float dropSpeed;
    float gravMult = 1;

    /// <summary>
    /// Enables the drop input action
    /// </summary>
    private void Awake()
    {
        actionMap = GetComponent<PlayerInput>().currentActionMap;
        actionMap.Enable();

        drop = actionMap.FindAction("Drop");

        drop.started += Drop_started;
    }

    /// <summary>
    /// Detects the ground and moves the disc downwards
    /// </summary>
    /// <param name="obj"></param>
    private void Drop_started(InputAction.CallbackContext obj)
    {
        if(FindGround())
        {
            rb.velocity = Vector3.zero;

            //Makes it so you can't have 0 or negative gravity
            if (dropSpeed > 0)
            {
                gravMult = dropSpeed;
            }
        }
    }

    /// <summary>
    /// Adds gravity to the disc but the gravity value can be changed
    /// </summary>
    protected override void DiscGravity()
    {
        rb.AddForce(Vector3.down * (rb.mass * discGravity) * gravMult);
    }

    /// <summary>
    /// Sends out a box cast to where the ground is
    /// </summary>
    /// <returns></returns>
    private bool FindGround()
    {
        return Physics.BoxCast(discBox.bounds.center, discBox.bounds.size, Vector3.down, Quaternion.identity, Mathf.Infinity);
    }

    /// <summary>
    /// Disables the drop input action
    /// </summary>
    private void OnDisable()
    {
        actionMap.Disable();
        drop.started -= Drop_started;
    }
}
