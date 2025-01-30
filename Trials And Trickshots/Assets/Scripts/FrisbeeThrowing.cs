/******************************************************************************
 * Author: Sky Turner
 * Creation Date: 1/30/2025
 * File Name: CameraBehavior.cs
 * Brief: Controls the camera
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.Experimental.GraphView;

public class FrisbeeThrowing : MonoBehaviour
{
    [SerializeField] GameObject frisbeePrefab;
    [SerializeField] Transform frisbeeInstantiationPoint;
    [SerializeField] Transform player;
    [SerializeField] int frisbeeForce;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ThrowFrisbee();
        }
    }

    private void ThrowFrisbee()
    {
        GameObject frisbee = Instantiate(frisbeePrefab, frisbeeInstantiationPoint);
        frisbee.transform.SetParent(null);
        Rigidbody frisbeeRB = frisbee.GetComponent<Rigidbody>();
        frisbeeRB.AddForce((frisbeeInstantiationPoint.position - player.position) * frisbeeForce);
    }

}
