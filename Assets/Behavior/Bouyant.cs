using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouyant : MonoBehaviour
{
    Rigidbody rb;
    public float upwardForce = 20;

    //A Higher Number means higher drag
    public float drag = 1;
    public float angularDrag = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < 0)
        {
            rb.AddForce(new Vector3(0f, upwardForce, 0f));
            rb.AddForce(rb.velocity * -1 * drag);
            rb.AddTorque(rb.angularVelocity * -1 * angularDrag * rb.mass);
        }
    }
}
