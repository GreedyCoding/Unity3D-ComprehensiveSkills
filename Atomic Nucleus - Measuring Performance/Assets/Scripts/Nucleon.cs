using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//If the rigidbody component is not found it gets created
[RequireComponent(typeof(Rigidbody))]
public class Nucleon : MonoBehaviour
{
    //References for the unity Inspector
    public float attractionForce;

    //Reference for the rigidbody
    Rigidbody rb;

    private void Awake()
    {
        //Store the rigidbody component in rb to manipulate it
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //Adding a force to the rigidbody that pushes him away from his current position
        rb.AddForce(transform.localPosition * -attractionForce);
    }
}
