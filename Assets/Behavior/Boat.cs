using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boat : MonoBehaviour
{

    Rigidbody rb;
    public Canvas Canvas;
    public Text text;
    public Text debug;
    bool playerBoarded = false;
    GameObject player;
    public GameObject camera;

    float timeCounter;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //text = Canvas.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCounter > 0)
            timeCounter -= Time.deltaTime;

        if (playerBoarded && timeCounter <= 0)
        {
            debug.text = "press b to dismount";
        }
            

        if (Input.GetKeyDown(KeyCode.B) && playerBoarded && timeCounter <= 0)
        {
            player.transform.position = gameObject.transform.position + new Vector3(0, 2, 0);
            player.transform.rotation = gameObject.transform.rotation;

            player.SetActive(true);

            camera.GetComponent<Camera>().enabled = false;
            player.transform.GetChild(0).GetComponent<Camera>().enabled = true;
            playerBoarded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            text.text = "Press b to board";
            player = other.gameObject;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.B) && !playerBoarded)
        {
            //other.gameObject.GetComponent<Character>().boat = gameObject;
            camera.GetComponent<Camera>().enabled = true;
            player.transform.GetChild(0).GetComponent<Camera>().enabled = false;
            player.transform.GetChild(0).GetComponent<AudioListener>().enabled = false;
            camera.GetComponent<AudioListener>().enabled = true;
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
            text.text = "";
        }
    }
}
