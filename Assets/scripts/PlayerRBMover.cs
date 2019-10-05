using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRBMover : MonoBehaviour {

    private Rigidbody rb;
    private Camera cam;
    private Inventory inv;
    private SpriteRenderer cursor;

    private int DEFAULT_MASK, INTERACTABLE_MASK, OBSERVABLE_MASK;

    public Sprite cursorNone;
    public Sprite cursorGrab;
    public Sprite cursorSee;

    public float SPEED = 10;
    public float ACCEL = 10;
    public float JUMPNESS = 5;
    public float INTERACTION_DIST = 5;
    public float OBSERVATION_DIST = 5;

	// Use this for initialization
	void Start () {
        rb = transform.GetComponent<Rigidbody>();
        cam = transform.GetComponentInChildren<Camera>();
        inv = transform.GetComponent<Inventory>();
        cursor = transform.Find("Main Camera").Find("crosshair").GetComponent<SpriteRenderer>();

        DEFAULT_MASK = LayerMask.GetMask("Default");
        INTERACTABLE_MASK = LayerMask.GetMask("Interactable");
        OBSERVABLE_MASK = LayerMask.GetMask("Observable");
        //relock in case
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("masks:");
        Debug.Log(DEFAULT_MASK);
        Debug.Log(INTERACTABLE_MASK);
        Debug.Log(OBSERVABLE_MASK);
	}
	
	// Update is called once per frame
	void Update () {
        handleMove();
        handleInteract();
	}

    private void handleMove()
    {
        float moveX;
        float moveY;
        float lookX;
        float lookY;
        bool onGround = Physics.Raycast(transform.position, Vector3.down, 1.25f, DEFAULT_MASK);

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

        transform.Rotate(Vector3.up, lookX);
        if (!((cam.transform.rotation.eulerAngles.x > 80 && cam.transform.rotation.eulerAngles.x < 100 && lookY > 0)
            || (cam.transform.rotation.eulerAngles.x > 260 && cam.transform.rotation.eulerAngles.x < 280 && lookY < 0)))
        {
            cam.transform.Rotate(cam.transform.right, lookY, Space.World);
        }

        if (onGround && Input.GetButtonDown("Jump"))
        {
            rb.velocity = rb.velocity + Vector3.up * JUMPNESS;
        }

        if (Input.GetButtonDown("Interact"))
        {
            //always try to lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void handleInteract()
    {
        bool intPressed = Input.GetButtonDown("Interact");
        bool dropPressed = Input.GetButtonDown("Drop");
        bool obsPressed = Input.GetButtonDown("Observe");
        if (intPressed)
        {
            //always try to lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //drop first idk
        if (dropPressed)
        {
            inv.dropCurrent();
        }

        //raycast to object?
        RaycastHit hitInfoInteract, hitInfoObserve;
        bool didHitInteract = Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfoInteract, INTERACTION_DIST, INTERACTABLE_MASK);
        bool didHitObserve = Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfoObserve, OBSERVATION_DIST, OBSERVABLE_MASK);
        //Debug.DrawRay(cam.transform.position, cam.transform.forward);
        if (didHitInteract)
        {
            cursor.sprite = cursorGrab;
            if (intPressed)
            {
                Interaction i = hitInfoInteract.collider.GetComponent<Interaction>();
                if (i == null)
                {
                    Debug.Log("No interaction found!");
                }
                else if(i.pickUp)
                {
                    inv.addObject(hitInfoInteract.collider.transform.parent.gameObject); //interactions always children of objects.
                }
                else
                {
                    i.doAction();
                }
            }
        }
        else if (didHitObserve)
        {
            cursor.sprite = cursorSee;
        }
        else
        {
            cursor.sprite = cursorNone;
        }

        //switch objects
        float scroll = Input.GetAxis("Scroll");
        if (scroll > 0.01f)
        {
            inv.selectNext();
        }
        else if (scroll < -0.01f)
        {
            inv.selectPrev();
        }
    }
}
