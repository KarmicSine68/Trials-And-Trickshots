/******************************************************************************
 * Author: Brad Dixon
 * Creation Date: 1/30/2025
 * File Name: Disc.cs
 * Brief: The basic class that all discs will derive from
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Disc : MonoBehaviour
{
    InputActionMap actionMap;
    InputAction throwDisc;

    GameObject player;

    [Header("Disc Variables")]

    [Tooltip("The disk prefab")]
    [SerializeField] private GameObject disc;

    [Tooltip("The base launch power of the disk")]
    [SerializeField] private float baseLaunchPower;

    [Tooltip("The multipler for the force of a max power throw")]
    [SerializeField] private float maxThrowMultipler;

    [Tooltip("How long it takes for a max throw to be reached")]
    [SerializeField] private float timeTillMaxPower;

    private float throwMultipler;
    private const float MIN_THROW_MULTIPLIER = 1;

    private bool chargingDisc;
    private bool discReady;

    [Header("Reset Condition")]
    [Tooltip("Check true if player is in the hub/firing range")]
    [SerializeField] private bool inHub;

    /// <summary>
    /// Gets a reference to the player's input actions
    /// </summary>
    private void Awake()
    {
        player = FindObjectOfType<PlayerBehaviour>().gameObject;

        //Enables the action map
        actionMap = player.GetComponent<PlayerInput>().currentActionMap;
        actionMap.Enable();

        throwDisc = actionMap.FindAction("Throw");

        throwDisc.started += ThrowDisc_started;
        throwDisc.canceled += ThrowDisc_canceled;

        discReady = true;
    }

    /// <summary>
    /// Executes code to charge the disc throw
    /// </summary>
    /// <param name="obj"></param>
    private void ThrowDisc_started(InputAction.CallbackContext obj)
    {
        if (discReady)
        {
            discReady = false;
            chargingDisc = true;
            StartCoroutine(ChargeDisc());
        }
    }

    /// <summary>
    /// Executes code to throw the disc
    /// </summary>
    /// <param name="obj"></param>
    private void ThrowDisc_canceled(InputAction.CallbackContext obj)
    {
        chargingDisc = false;
    }

    /// <summary>
    /// Code to increase the power of the disc
    /// </summary>
    private IEnumerator ChargeDisc()
    {
        while(chargingDisc)
        {
            if(!chargingDisc)
            {
                continue;
            }

            yield return new WaitForSeconds(.1f);
        }

        ThrowDisc();
    }

    /// <summary>
    /// Code to throw the disc
    /// </summary>
    private void ThrowDisc()
    {
        //Spawns the disc
        GameObject spawnedDisc = Instantiate(disc, player.transform);

        Vector3 launchForce = player.transform.forward * baseLaunchPower * throwMultipler;

        //Adds a force to the disc to get it to fly forward
        spawnedDisc.GetComponent<Rigidbody>().AddForce(launchForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Tells the code that a disc can be thrown again after the previous disc has landed
    /// </summary>
    public void ReadyDisc()
    {
        discReady = true;
    }
}
