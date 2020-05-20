using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    Rigidbody rb;
    GameObject waterPlane;
    bool waterPlaneSet;

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
