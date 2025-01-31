/******************************************************************************
 * Author: Sky Turner
 * Creation Date: 1/30/2025
 * File Name: DiscThrowing.cs
 * Brief: Handles the throwing of the disc.
 * ***************************************************************************/
using UnityEngine;

public class DiscThrowing : MonoBehaviour
{
    [SerializeField] GameObject discPrefab;
    [SerializeField] Transform discInstantiationPoint;
    [SerializeField] Transform player;
    [SerializeField] int discForce;


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
        GameObject frisbee = Instantiate(discPrefab, discInstantiationPoint);
        frisbee.transform.SetParent(null);
        Rigidbody frisbeeRB = frisbee.GetComponent<Rigidbody>();
        frisbeeRB.AddForce((discInstantiationPoint.position - player.position) * discForce);
    }

}
