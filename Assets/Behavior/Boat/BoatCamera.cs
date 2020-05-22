using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    float pitch = 0.0f;
    float yaw = 0.0f;

    float maxPitch = 80;
    float minPitch = -80;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch += speedV * Input.GetAxis("Mouse Y");

        pitch = (float)Clamp(maxPitch, minPitch, pitch);

        transform.eulerAngles = new Vector3(-1 * pitch, yaw, 0.0f);
    }

    T Clamp<T>(T max, T min, T val) where T : IComparable<T>
    {
        if(val.CompareTo(max) > 0)
            return max;
        else if(val.CompareTo(min) < 0)
            return min;
        else return val;
    }
}
