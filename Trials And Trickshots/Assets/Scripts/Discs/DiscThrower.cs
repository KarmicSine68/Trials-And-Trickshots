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
using UnityEngine.UI;

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

    [Tooltip("The height offset for where the disc gets thrown")]
    [SerializeField] private float yOffset;

    private float throwMultipler;
    private const float MIN_THROW_MULTIPLIER = 1;

    private bool chargingDisc;
    private bool discReady;

    [Header("Reset Condition")]
    [Tooltip("Check true if player is in the hub/firing range")]
    [SerializeField] private bool inHub;

    //Charge meter
    [SerializeField] private GameObject chargeMeter;
    [SerializeField] private GameObject chargeBar;
    Image chargeFill;

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

        GameObject[] objects = FindObjectsOfType<GameObject>();
        foreach(GameObject i in objects)
        {
            if(i.name.Contains("ChargeMeter"))
            {
                chargeMeter = i;
            }

            if(i.name.Contains("Fill"))
            {
                chargeBar = i;
            }
        }

        chargeFill = chargeBar.GetComponent<Image>();
        chargeFill.fillAmount = 0;
    }

    /// <summary>
    /// Displays the charge bar
    /// </summary>
    private void Update()
    {
        if(discReady)
        {
            chargeMeter.SetActive(true);
        }

        if(chargingDisc)
        {
            chargeBar.SetActive(true);

            float work = (throwMultipler - 1) / (maxThrowMultipler - 1);

            Debug.Log(work);

            chargeFill.fillAmount =
                Mathf.Lerp(chargeFill.fillAmount, work, 10 * Time.deltaTime);

            Color chargeColor = Color.Lerp(Color.green, Color.red, work);

            chargeFill.color = chargeColor;
        }
        else if(!discReady)
        {
            chargeBar.SetActive(false);
            chargeMeter.SetActive(false);
            chargeFill.fillAmount = 0;
        }
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
        bool incrementing = true;
        float changeValue = (maxThrowMultipler - MIN_THROW_MULTIPLIER) / (timeTillMaxPower * 10);

        //Slight delay before charging begins
        yield return new WaitForSeconds(.1f);

        while (chargingDisc)
        {
            if(!chargingDisc)
            {
                continue;
            }

            yield return new WaitForSeconds(.1f);

            if (incrementing)
            {
                throwMultipler += changeValue;
                if (throwMultipler >= maxThrowMultipler)
                {
                    throwMultipler = maxThrowMultipler;
                    incrementing = false;
                }
            }
            //Starts decreasing in power when max power has been achieved
            else
            {
                throwMultipler -= changeValue;
                if (throwMultipler <= MIN_THROW_MULTIPLIER)
                {
                    throwMultipler = MIN_THROW_MULTIPLIER;
                    incrementing = true;
                }
            }
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
        fakeDiscs[listIndex].SetActive(false);

        Vector3 offsetVector = new Vector3(0, yOffset, 0);

        //Spawns the disc
        GameObject spawnedDisc = Instantiate(discs[listIndex], cam.transform.position + offsetVector, Quaternion.identity);

        Vector3 launchForce = cam.transform.forward * baseLaunchPower * throwMultipler;

        //Adds a force to the disc to get it to fly forward
        spawnedDisc.GetComponent<Rigidbody>().AddForce(launchForce, ForceMode.Impulse);

        //increases the count on the scenes score manager
        try
        {
            GameObject.Find("ScoreManager").GetComponent<ScoreManager>().IncreaseThrows();
        }
        catch
        {

        }

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
