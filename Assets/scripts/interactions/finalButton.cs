using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalButton : Interaction {

    private keypad final;
    private AudioManager am;

    public AudioClip typo;
    public AudioClip copy;

	// Use this for initialization
	void Start () {
        final = GameObject.FindGameObjectWithTag("final").GetComponent<keypad>();
        am = GameObject.FindObjectOfType<AudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void doAction()
    {
        if (final.getDone())
        {
            am.PlayThought(typo);
        }
        else
        {
            am.PlayThought(copy);
        }
        base.doAction();
    }
}
