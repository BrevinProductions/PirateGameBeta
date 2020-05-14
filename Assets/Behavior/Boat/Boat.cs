using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boat : MonoBehaviour
{

    Rigidbody rb;
    public Text debug;
    bool playerBoarded = false;
    GameObject player;
    public Transform pCamera;
    bool anchorRaised = false;
    public float forwardForce = 30;
    public float turnForce = 10;

    float timeCounter;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCounter > 0)
            timeCounter -= Time.deltaTime;

        if (playerBoarded && timeCounter <= 0)
        {
            debug.text = "press b to dismount";
            pCamera = transform.GetChild(0);
        }
            
        //Dismounting Boat
        if (Input.GetKeyDown(KeyCode.B) && playerBoarded && timeCounter <= 0)
        {
            player.transform.position = gameObject.transform.position + new Vector3(0, 2, 0);
            player.transform.rotation = gameObject.transform.rotation;

            player.SetActive(true);

            pCamera.GetComponent<Camera>().enabled = false;
            player.transform.GetChild(0).GetComponent<Camera>().enabled = true;
            playerBoarded = false;
            timeCounter = 1;
        }

        //Ship Control
        if (playerBoarded)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                anchorRaised = true;
            }
            if (anchorRaised)
            {
                rb.AddRelativeForce(Vector3.forward * forwardForce);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddTorque(Vector3.up * turnForce);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddTorque(Vector3.up * turnForce * -1);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            debug.text = "Press b to board";
            player = other.gameObject;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("Player") && Input.GetKey(KeyCode.B) && !playerBoarded && timeCounter <= 0)
        {
            //other.gameObject.GetComponent<Character>().boat = gameObject;
            pCamera.GetComponent<Camera>().enabled = true;
            player.transform.GetChild(0).GetComponent<Camera>().enabled = false;
            player.transform.GetChild(0).GetComponent<AudioListener>().enabled = false;
            pCamera.GetComponent<AudioListener>().enabled = true;
            player.SetActive(false);
            playerBoarded = true;

            //locks player into boat for 1 second
            timeCounter = 1f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            debug.text = "";
        }
    }
}
