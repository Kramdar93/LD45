using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarBehavior : MonoBehaviour {

    public float MIN_DIST;
    public float MAX_DIST;

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = Component.FindObjectOfType<PlayerRBMover>().gameObject;

        //lay em down
        transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 newRot = new Vector3(0,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z);
        //Debug.Log(transform.eulerAngles);
        //Debug.Log(distance);
        // don't do math for large distances
        //if (distance > MAX_DIST *1.1f)
        //{
        //    return;
        //}
        if (distance > MAX_DIST)
        {
            transform.eulerAngles = newRot;
        }
        else if (distance > MIN_DIST)
        {
            newRot += Vector3.right * (90-(90 * ((distance - MIN_DIST) / (MAX_DIST - MIN_DIST))));
            //Debug.Log((90 * ((distance - MIN_DIST) / (MAX_DIST - MIN_DIST))));
            transform.eulerAngles = newRot;
        }
        else
        {
            newRot += Vector3.right * 90;
            transform.eulerAngles = newRot;
        }
	}
}
