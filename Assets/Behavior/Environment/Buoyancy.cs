using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    Rigidbody rb;
    GameObject waterPlane;
    bool waterPlaneSet;
    public float upwardForce = 30f;
    public float dragForce = 10;
    public float angDragForce = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waterPlaneSet)
        {
            if(gameObject.transform.position.y < waterPlane.transform.position.y)
            {
                rb.AddForce(Vector3.up * upwardForce);
                rb.AddForce(rb.velocity * -1 * dragForce);
                rb.AddTorque(rb.angularVelocity * -1 * angDragForce);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Water"))
        {
            waterPlane = other.gameObject;
            waterPlaneSet = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals("Water"))
        {
            waterPlane = null;
            waterPlaneSet = false;
        }
    }
}
