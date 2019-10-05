using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapped : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	
	void FixedUpdate () {
        //Debug.Log(transform.position);
        if (transform.position.x > 20)
        {
            transform.position = transform.position + new Vector3(-40,0,0);
        }
        if (transform.position.z > 20)
        {
            transform.position = transform.position + new Vector3(0, 0, -40);
        }
        if (transform.position.x < -20)
        {
            transform.position = transform.position + new Vector3(40, 0, 0);
        }
        if (transform.position.z < -20)
        {
            transform.position = transform.position + new Vector3(0, 0, 40);
        }
	}
}
