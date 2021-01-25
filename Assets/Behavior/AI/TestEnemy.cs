using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEnemy : MonoBehaviour
{
  public byte HEALTH = 2;
  public GameObject player;

  bool chase = false;

  Rigidbody body;
  void Start()
  {
    //set local rigidbody
    body = this.gameObject.GetComponent<Rigidbody>();
  }
  void Update()
  {
    if(HEALTH == 0)
    {
      gameObject.SetActive(false);
    }
    if(chase)
    {
      transform.LookAt(player.transform, Vector3.up);
      transform.localPosition += Vector3.right * Time.deltaTime * 2;
    }
  }
  void OnTriggerEnter(Collider other)
  {
    if(other.gameObject.tag.Equals("Sword"))
    {
      if((other.gameObject.GetComponent<PlayerSword>().swingTimer > 0) && (HEALTH > 0))
      {
        HEALTH -= 1;
      }
    }
    if (other.gameObject.tag.Equals("Player"))
    {
      chase = true;
      player = other.gameObject;
    }
  }

  void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag.Equals("Player"))
    {
      chase = false;
      player = null;
    }
  }
}
