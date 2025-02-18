/******************************************************************************
 * Author: Brad Dixon
 * Creation Date: 1/30/2025
 * File Name: DiscThrower.cs
 * Brief: The basic class that all discs will derive from
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiscThrower : MonoBehaviour
{
    InputActionMap actionMap;
    InputAction throwDisc, cycleUp, cycleDown;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cam;

    [Header("Disc Variables")]

    [Tooltip("List of visual objects of the discs")]
    [SerializeField] private List<GameObject> fakeDiscs = new List<GameObject>();

    [Tooltip("List of disc prefabs")]
    [SerializeField] private List<GameObject> discs = new List<GameObject>();
    private int listIndex = 0;

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
        
        //Enables the action map
        actionMap = player.GetComponent<PlayerInput>().currentActionMap;
        actionMap.Enable();

        throwDisc = actionMap.FindAction("Throw");
        cycleUp = actionMap.FindAction("CycleUp");
        cycleDown = actionMap.FindAction("CycleDown");

        throwDisc.started += ThrowDisc_started;
        throwDisc.canceled += ThrowDisc_canceled;

        cycleUp.started += CycleUp_started;
        cycleDown.started += CycleDown_started;

        discReady = true;
        DisplayDisc();
    }

    /// <summary>
    /// Cycles down through the list of discs and wraps back to the top
    /// </summary>
    /// <param name="obj"></param>
    private void CycleDown_started(InputAction.CallbackContext obj)
    {
        --listIndex;
        if(listIndex < 0)
        {
            listIndex = discs.Count - 1;
        }

        DisplayDisc();
    }

    /// <summary>
    /// Cycles up through the list of discs and wraps back to the bottom
    /// </summary>
    /// <param name="obj"></param>
    private void CycleUp_started(InputAction.CallbackContext obj)
    {
        ++listIndex;
        if(listIndex >= discs.Count)
        {
            listIndex = 0;
        }

        DisplayDisc();
    }

    /// <summary>
    /// Executes code to charge the disc throw
    /// </summary>
    /// <param name="obj"></param>
    private void ThrowDisc_started(InputAction.CallbackContext obj)
    {
        if (discReady)
        {
            fakeDiscs[listIndex].SetActive(false);

            discReady = false;
            chargingDisc = true;

            //Defaults to the minimum throw multiplier
            throwMultipler = MIN_THROW_MULTIPLIER;

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

        //if(inHub)
        //{
        //    discReady = true;
        //}
    }

    /// <summary>
    /// Code to throw the disc
    /// </summary>
    private void ThrowDisc()
    {
        Debug.Log("Reached");

        //Spawns the disc
        GameObject spawnedDisc = Instantiate(discs[listIndex], cam.transform.position, Quaternion.identity);

        Vector3 launchForce = cam.transform.forward * baseLaunchPower * throwMultipler;

        Debug.Log(launchForce);

        //Adds a force to the disc to get it to fly forward
        spawnedDisc.GetComponent<Rigidbody>().AddForce(launchForce, ForceMode.Impulse);

        //increases the count on the scenes score manager
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().IncreaseThrows();

        //An alternte way of throwing the disc in case the previous way causes problems
        //float launchForce = baseLaunchPower * throwMultipler;
        //spawnedDisc.GetComponent<Rigidbody>().velocity = player.transform.forward * launchForce;
    }

    /// <summary>
    /// Tells the code that a disc can be thrown again after the previous disc has landed
    /// </summary>
    public void ReadyDisc()
    {
        discReady = true;
        DisplayDisc();
    }

    /// <summary>
    /// Displays which visual disc should be shown
    /// </summary>
    private void DisplayDisc()
    {
        foreach(GameObject i in fakeDiscs)
        {
            if(fakeDiscs.IndexOf(i) == listIndex)
            {
                i.SetActive(true);
            }
            else
            {
                i.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        actionMap.Disable();

        throwDisc.started -= ThrowDisc_started;
        throwDisc.canceled -= ThrowDisc_canceled;

        cycleUp.started -= CycleUp_started;
        cycleDown.started -= CycleDown_started;
    }
}
