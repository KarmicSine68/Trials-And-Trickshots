using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Vector3 horse;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(horse * Time.deltaTime);
    }
}
