using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRBMover : MonoBehaviour {

    private Rigidbody rb;
    private Camera cam;

    public float SPEED = 10;
    public float ACCEL = 10;
    public float JUMPNESS = 5;
    public float INTERACTION_DIST = 5;

	// Use this for initialization
	void Start () {
        rb = Component.FindObjectOfType<Rigidbody>();
        cam = transform.GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        float moveX;
        float moveY;
        float lookX;
        float lookY;
        bool onGround = Physics.Raycast(transform.position, Vector3.down,1.25f);

        moveX = Input.GetAxis("MoveX");
        moveY = Input.GetAxis("MoveY");
        lookX = Input.GetAxis("LookX");
        lookY = -Input.GetAxis("LookY");

        if (onGround)
        {
            float oldUp = rb.velocity.y;
            rb.velocity = ((transform.forward * moveY) + (transform.right * moveX)).normalized * SPEED + (Vector3.up * oldUp);
        }
        else //midair controls
        {
            Debug.Log(Vector3.Dot(rb.velocity, transform.right) + ", " + Vector3.Dot(rb.velocity, transform.forward));
            Vector3 result = Vector3.zero;
            if (!((Vector3.Dot(rb.velocity, transform.right) > SPEED && moveX > 0) || (Vector3.Dot(rb.velocity, transform.right) < -SPEED && moveX < 0)))
            {
                result += cam.transform.right.normalized * moveX;
            }
            if (!((Vector3.Dot(rb.velocity, transform.forward) > SPEED && moveY > 0) || (Vector3.Dot(rb.velocity, transform.forward) < -SPEED && moveY < 0)))
            {
                result += cam.transform.forward.normalized * moveY;
            }

            if (result.magnitude > 0.1f)
            {
                rb.AddForce(result.normalized * ACCEL, ForceMode.Acceleration);
            }
        }

        transform.Rotate(Vector3.up,lookX);
        if (!((cam.transform.rotation.eulerAngles.x > 80 && cam.transform.rotation.eulerAngles.x < 100 && lookY > 0)
            || (cam.transform.rotation.eulerAngles.x > 260 && cam.transform.rotation.eulerAngles.x < 280 && lookY < 0)))
        {
            cam.transform.Rotate(cam.transform.right, lookY, Space.World); 
        }

        if (onGround && Input.GetButtonDown("Jump"))
        {
            rb.velocity = rb.velocity + Vector3.up * JUMPNESS;
        }


	}
}
