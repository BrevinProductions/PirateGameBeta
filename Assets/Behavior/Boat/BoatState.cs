using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatState : MonoBehaviour {

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

  byte rudderAngle = 0; //
  byte maxRudderAngle = 80;
  bool piloted = false;
  float unanchorCount = 0;
  float undockCount = 0;
  float anchorCount = 0;

  enum RudderState : byte {
    STATE_RUDDER_CENTER,
    STATE_RUDDER_RIGHT,
    STATE_RUDDER_LEFT
  }

  // Start is called before the first frame update
  void Start () {
    moveState = MovementState.STATE_ANCHORED;
  }

  // Update is called once per frame
  void Update () {
    switch (moveState) {
      case MovementState.STATE_DOCKED:
        //test for undocking input and handle
        if(piloted && Input.GetKeyDown(KeyCode.R)){
          moveState = MovementState.STATE_UNDOCK;
          //set count to undock
          undockCount = 20;
        }
        break;
      case MovementState.STATE_ANCHORED:
        //test for unanchoring
        if(piloted && Input.GetKeyDown(KeyCode.R)){
          moveState = MovementState.STATE_UNANCHOR;
          //set time to unanchor
          unanchorCount = 10;
        }
        break;
      case MovementState.STATE_MOVING:
        //test for input for docking
        if(canDock && Input.GetKeyDown(KeyCode.R))
        {
          moveState = MovementState.STATE_DOCKING;
          break;
        }
        //if docking
        //test for input for anchoring
        else if(Input.GetKeyDown(KeyCode.R)){
          moveState = STATE_ANCHORING;
          anchorCount = 10;
          break;
        }

        //ensure rudder cannot turn past 80:
        rudderAngle = (byte)Clamp(80, 0, rudderAngle);
        //if anchoring
        switch (rudderState)
        {
          case RudderState.STATE_RUDDER_CENTER:
            //handle move forward
            break;
          case RudderState.STATE_RUDDER_RIGHT:
            //handle right turn
            break;
          case RudderState.STATE_RUDDER_LEFT:
            //handle right turn
            break;
          default:
            break;
        }
      case MovementState.STATE_ANCHORING:
        //handle anchoring
        anchorCount -= Time.deltaTime;
        //test for anchoring finished and switch to anchored
        if(anchorCount <= 0)
          moveState = STATE_ANCHORED;
        break;
      case MovementState.STATE_DOCKING:
        //handle docking
        //move to docked position
        //test for docking finished and switch to docked
        break;
      case MovementState.STATE_UNANCHOR:
        //handle unanchoring
        //test for process finished and switch to moving
        break;
      case MovementState.STATE_UNDOCK:
        //handle undock
        //test for undocking finished and switch to moving
        break;
      case MovementState.STATE_SINKING:
        //test for repaired (and switch state to moving, set health to reapairedHealth
        //test for sunken state (Switch to STATE_SUNK)

        //countdown sinking timer
        break;
      case MovementState.STATE_SUNK:
        //kinda do nothing
        //countdown to ship despawn
        //test for ship despawn
        break;
      default:
        break;
    }
  }

  T Clamp(T max, T min, T val) where T : IComparable
  {
    if(max.CompareTo(val) > 0)
      return max;
    else if(min.CompareTo(val) < 0)
      return min;
    else
      return val;
  }
}
