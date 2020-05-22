using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatState : MonoBehaviour {

  //
  //TODO: 
  //ADD STATE_RUN_AGGROUND
  //IMPLEMENT DOCKING SEQUENCE
  //ADD PLAYER DISMOUNTING
  //

  Rigidbody rigidbody;

  enum MovementState : byte {
    STATE_DOCKED,
    STATE_ANCHORED,
    STATE_MOVING,
    STATE_ANCHORING,
    STATE_DOCKING,
    STATE_UNANCHOR,
    STATE_UNDOCK,
    STATE_SINKING,
    STATE_SUNK
  }

  MovementState moveState;
  RudderState rudderState;

  GameObject player;

  //sink the ship
  public void SinkShip(){
    moveState = MovementState.STATE_SINKING;
    sinkingCount = maxSinkTime;
    repaired = false;
  }

  float rudderAngle = 0; //how far the rudder is turned
  byte maxRudderAngle = 8; //how far the rudder can turn
  bool piloted = false; //if the ship has a captain at the helm


  //start counts
  float unanchorCount = 0;
  float undockCount = 0;
  float anchorCount = 0;
  float despawnCount = 0;
  float sinkingCount = 0;
  float cooldown = 0;
  //end counts

  //public fields
  public const float maxSinkTime = 120;
  public float windForce = 20;
  public Text text;
  
  //defines repaired state
  public bool repaired = true;
  bool canDock;

  enum RudderState : byte {
    STATE_RUDDER_CENTER,
    STATE_RUDDER_RIGHT,
    STATE_RUDDER_LEFT
  }

  // Start is called before the first frame update
  void Start () {
    moveState = MovementState.STATE_ANCHORED;
    rudderState = RudderState.STATE_RUDDER_CENTER;
    rigidbody = gameObject.GetComponent<Rigidbody>();
    transform.GetChild(0).GetComponent<BoatCamera>().enabled = false;
  }

  // Update is called once per frame
  void Update () 
  {
    //text.text = "";

    if(cooldown > 0)
    {
      cooldown -= Time.deltaTime;
    }

    switch (moveState) {
      case MovementState.STATE_DOCKED:
        //test for undocking input and handle
        if(piloted && Input.GetKey(KeyCode.R)){
          moveState = MovementState.STATE_UNDOCK;
          //set count to undock
          undockCount = 20;
        }
        break;
      case MovementState.STATE_ANCHORED:
        //test for unanchoring
        if(piloted && Input.GetKey(KeyCode.R)){
          moveState = MovementState.STATE_UNANCHOR;
          //set time to unanchor
          unanchorCount = 10;
        }
        break;
      
       case MovementState.STATE_MOVING:
        //test for input for docking
        if(canDock && Input.GetKey(KeyCode.R))
        {
          moveState = MovementState.STATE_DOCKING;
          break;
        }
        //if docking
        //test for input for anchoring
        else if(Input.GetKey(KeyCode.R)){
          moveState = MovementState.STATE_ANCHORING;
          anchorCount = 10;
          break;
        }

        //ensure rudder cannot turn past 80:
        
        rigidbody.AddRelativeForce(Vector3.forward * windForce);

        if(rudderAngle == 0)
        {
          rudderState = RudderState.STATE_RUDDER_CENTER;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (rudderState == RudderState.STATE_RUDDER_LEFT && rudderAngle > 0)
              rudderAngle -= 2.0f * Time.deltaTime;
            else if (rudderState == RudderState.STATE_RUDDER_CENTER)
            {
              rudderAngle += 1.0f * Time.deltaTime;
              rudderState = RudderState.STATE_RUDDER_RIGHT;
            }
            else if(rudderState == RudderState.STATE_RUDDER_RIGHT)
              rudderAngle += 1.0f * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
          if (rudderState == RudderState.STATE_RUDDER_RIGHT && rudderAngle > 0)
            rudderAngle -= 2.0f * Time.deltaTime;
          else if (rudderState == RudderState.STATE_RUDDER_CENTER)
          {
              rudderAngle += 1.0f * Time.deltaTime;
              rudderState = RudderState.STATE_RUDDER_LEFT;
          }
          else if(rudderState == RudderState.STATE_RUDDER_LEFT)
            rudderAngle += 1.0f * Time.deltaTime;
        }
        else
          rudderAngle -= 1.0f * Time.deltaTime;
        
        rudderAngle = (float)Clamp(maxRudderAngle, 0, rudderAngle);

        switch (rudderState)
        {
          case RudderState.STATE_RUDDER_CENTER:
            //handle move forward and kill rotation
            //rigidbody.angularVelocity = Vector3.zero;
            rigidbody.AddTorque(rigidbody.angularVelocity * -20);
            break;
          case RudderState.STATE_RUDDER_RIGHT:
            rigidbody.AddTorque(Vector3.up * rudderAngle);
            text.text = rudderAngle.ToString();
            //handle right turn
            break;
          case RudderState.STATE_RUDDER_LEFT:
            rigidbody.AddTorque(Vector3.up * rudderAngle * -1);
            text.text = rudderAngle.ToString();
            //handle right turn
            break;
          default:
            break;
        }
      break;
      case MovementState.STATE_ANCHORING:
        //handle anchoring
        anchorCount -= Time.deltaTime;
        text.text = anchorCount.ToString();
        //test for anchoring finished and switch to anchored
        if(anchorCount <= 0)
          moveState = MovementState.STATE_ANCHORED;
        break;
      case MovementState.STATE_DOCKING:
        //handle docking
        //move to docked position
        //test for docking finished and switch to docked
        break;
      case MovementState.STATE_UNANCHOR:
        //handle unanchoring
        unanchorCount -= Time.deltaTime;
        text.text = unanchorCount.ToString();
        //test for process finished and switch to moving
        if(unanchorCount <= 0)
          moveState = MovementState.STATE_MOVING;
        break;
      case MovementState.STATE_UNDOCK:
        //handle undock
        undockCount -= Time.deltaTime;
        //test for undocking finished and switch to moving
        if(undockCount <= 0)
          moveState = MovementState.STATE_MOVING;
        break;
      case MovementState.STATE_SINKING:
        //test for repaired (and switch state to moving, set health to reapairedHealth
        sinkingCount -= Time.deltaTime;
        if(repaired)
          moveState = MovementState.STATE_MOVING;
        //test for sunken state (Switch to STATE_SUNK)
        if(sinkingCount <= 0){
          moveState = MovementState.STATE_SUNK;
          despawnCount = 600;
        }
        break;
      case MovementState.STATE_SUNK:
        //kinda do nothing
        //countdown to ship despawn
        despawnCount -= Time.deltaTime;
        //test for ship despawn
        if(despawnCount <= 0)
          gameObject.SetActive(false);
        break;
      default:
        break;
    }
  }


  void OnTriggerEnter(Collider other){
    if(other.gameObject.tag.Equals("Player")){
      text.text = "Press b to board";
      player = other.gameObject;
    }
  }

  void OnTriggerExit(Collider other){
    if(other.gameObject.tag.Equals("Player")){
      text.text = "";
    }
  }

  void OnTriggerStay(Collider other){
    if(other.gameObject.tag.Equals("Player") && 
    Input.GetKey(KeyCode.B) && 
    cooldown <= 0)
    {
      //set camera
      piloted = true;
      player.SetActive(false);
      text.text = "";
      cooldown = 2;
      transform.GetChild(0).GetComponent<Camera>().enabled = true;
      transform.GetChild(0).GetComponent<BoatCamera>().enabled = true;
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
