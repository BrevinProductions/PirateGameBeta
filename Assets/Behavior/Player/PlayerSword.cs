using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSword : MonoBehaviour
{
    public float swingTimer { get; set; } = 0;

    void Update()
    {
        if (swingTimer > 0f)
        {
          swingTimer -= Time.deltaTime;
        }
        swingTimer = Clamp(255f, 0f, swingTimer);

        if (Input.GetMouseButtonDown(0) && swingTimer >= 0)
        {
          swingTimer = 0.5f;
          this.gameObject.GetComponent<Animation>().Play();
        }
    }
    T Clamp<T>(T max, T min, T val) where T : IComparable<T>
    {
      if(val.CompareTo(max) > 0)
        return max;
      else if(val.CompareTo(min) < 0)
        return min;
      else
        return val;
    }
}
