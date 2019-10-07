using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRBMover : MonoBehaviour {

    private Rigidbody rb;
    private Camera cam;
    private Inventory inv;
    private SpriteRenderer cursor;
    private tutorial tut;
    private AudioManager am;

    private int DEFAULT_MASK, INTERACTABLE_MASK, OBSERVABLE_MASK;

    public Sprite cursorNone;
    public Sprite cursorGrab;
    public Sprite cursorSee;

    public float SPEED = 10;
    public float ACCEL = 10;
    public float JUMPNESS = 5;
    public float INTERACTION_DIST = 5;
    public float OBSERVATION_DIST = 5;

    private Terrain terrain;
    public void SetTerrain()
    {
        terrain = GameObject.FindObjectOfType<Terrain>();
    }

    void Awake()
    {
        //keep whole hierarchy of player.
        DontDestroyOnLoad(transform.gameObject);
    }

	// Use this for initialization
	void Start () {
        rb = transform.GetComponent<Rigidbody>();
        cam = transform.GetComponentInChildren<Camera>();
        inv = transform.GetComponent<Inventory>();
        cursor = transform.Find("Main Camera").Find("crosshair").GetComponent<SpriteRenderer>();

        DEFAULT_MASK = LayerMask.GetMask("Default");
        INTERACTABLE_MASK = LayerMask.GetMask("Default","Interactable");
        OBSERVABLE_MASK = LayerMask.GetMask("Default", "Observable");
        //relock in case
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tut = Component.FindObjectOfType<tutorial>();
        if (tut == null)
        {
            Debug.Log("no tutorial found");
        }

        am = GameObject.FindObjectOfType<AudioManager>();

	}
	
	// Update is called once per frame
	void Update () {
        handleMove();
        handleInteract();
        //fix garbo
        if (terrain != null)
        {
            //Debug.Log(terrain.SampleHeight(transform.position));
            if (transform.position.y - (terrain.SampleHeight(transform.position)+1) < 0) // only works for player height of 1!
            {
                transform.position = transform.position + Vector3.up * ((terrain.SampleHeight(transform.position) + 1) - transform.position.y);
            }
        }
        //exit
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
	}

    private void handleMove()
    {
        float moveX;
        float moveY;
        float lookX;
        float lookY;
        bool onGround = Physics.Raycast(transform.position, Vector3.down, 1.25f, DEFAULT_MASK);
        bool sprinting = Input.GetButton("Sprint");

        moveX = Input.GetAxis("MoveX");
        moveY = Input.GetAxis("MoveY");
        lookX = Input.GetAxis("LookX") + 3 * Input.GetAxis("JoyLookX");
        lookY = -Input.GetAxis("LookY") + -1.5f * Input.GetAxis("JoyLookY");
        float speed = sprinting ? SPEED * 2 : SPEED;


        if (onGround)
        {
            float oldUp = rb.velocity.y;
            rb.velocity = ((transform.forward * moveY) + (transform.right * moveX)).normalized * speed + (Vector3.up * oldUp);
        }
        else //midair controls
        {
            Vector3 result = Vector3.zero;
            if (!((Vector3.Dot(rb.velocity, transform.right) > speed && moveX > 0) || (Vector3.Dot(rb.velocity, transform.right) < -speed && moveX < 0)))
            {
                result += cam.transform.right.normalized * moveX;
            }
            if (!((Vector3.Dot(rb.velocity, transform.forward) > speed && moveY > 0) || (Vector3.Dot(rb.velocity, transform.forward) < -speed && moveY < 0)))
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

        if (rb.velocity.magnitude > 0.1f && onGround)
        {
            am.ToggleFootsteps(true);
        }
        else
        {
            am.ToggleFootsteps(false);
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
        if (didHitObserve)
        {
            Observation obs = hitInfoObserve.collider.GetComponent<Observation>();
            if (obs != null)
            {
                cursor.sprite = cursorSee;
                if (obsPressed)
                {
                    Debug.Log("observing...");
                    obs.Observe();
                    if (tut != null)
                    {
                        tut.checkedWorld();
                    }
                }
            }
            else
            {
                didHitObserve = false;
            }
        }
        if (didHitInteract)
        {
            Interaction i = hitInfoInteract.collider.GetComponent<Interaction>();
            if (i != null)
            {
                cursor.sprite = cursorGrab;
                if (intPressed)
                {
                    if (i.pickUp)
                    {
                        inv.addObject(hitInfoInteract.collider.transform.parent.gameObject); //interactions always children of objects.
                    }
                    else
                    {
                        i.doAction();
                    }
                }
            }
            else
            {
                didHitInteract = false;
            }
        }
        if(!didHitObserve && !didHitInteract)
        {
            cursor.sprite = cursorNone;
        }

        //switch objects
        float scroll = Input.GetAxis("Scroll");
        bool nextButton = Input.GetButtonDown("Next");
        bool prevButton = Input.GetButtonDown("Prev");
        if (scroll > 0.01f || nextButton)
        {
            inv.selectNext();
            if (tut != null)
            {
                tut.checkedNote();
            }
        }
        else if (scroll < -0.01f || prevButton)
        {
            inv.selectPrev();
            if (tut != null)
            {
                tut.checkedNote();
            }
        }
    }

}
