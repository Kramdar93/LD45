using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outskirts : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("initializing");
        PlayerRBMover playa = Component.FindObjectOfType<PlayerRBMover>();
        playa.transform.position = GameObject.FindGameObjectWithTag("spawn").transform.position;
        playa.SetTerrain();
        GameObject[] cleanup = GameObject.FindGameObjectsWithTag("cleanMe");
        foreach(GameObject g in cleanup){
            Destroy(g);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
