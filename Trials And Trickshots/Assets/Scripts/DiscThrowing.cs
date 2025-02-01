/******************************************************************************
 * Author: Sky Turner
 * Creation Date: 1/30/2025
 * File Name: DiscThrowing.cs
 * Brief: Handles the throwing of the disc.
 * ***************************************************************************/
using UnityEngine;

public class DiscThrowing : MonoBehaviour
{
    [Header("Disc Settings")]
    [SerializeField] GameObject discPrefab;
    [SerializeField] Transform discInstantiationPoint;
    [SerializeField] Transform player;
    [SerializeField] int discForce;

    private void ThrowDisc()
    {
        if(discPrefab == null || discInstantiationPoint == null)
        {
            Debug.Log("Disc Prefab or Instantiation Point is missing.");
            return;
        }

        GameObject disc = Instantiate(discPrefab, discInstantiationPoint.position, discInstantiationPoint.rotation);
        disc.transform.SetParent(null);

        Rigidbody discRB = disc.GetComponent<Rigidbody>();
        if(discRB == null)
        {
            Debug.LogError("Disc Prefab is missing a Rigidbody component");
        }

        discRB.AddForce((discInstantiationPoint.position - player.position) * discForce);
    }

    public void RequestThrow()
    {
        ThrowDisc();
    }

}
