using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCountdown : MonoBehaviour {

    public AudioClip threeMin;
    public AudioClip thirtySec;
    public AudioClip dead;

    private AudioManager am;
    private float init;
    private bool ticking;
    private bool passed30;
    private bool isDead;


	// Use this for initialization
	void Start () {
        am = GameObject.FindObjectOfType<AudioManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if(ticking){
            float now = Time.time;
            if (now - init > 150 && !passed30)
            {
                am.PlayThought(thirtySec);
                passed30 = true;
                am.AdvanceTrack(false);
            }
            if (now - init > 180 && !isDead)
            {
                am.PlayThought(dead);
                isDead = true;
            }
            if (now - init > 200)
            {
                Application.Quit();
            }
        }
	}

    void OnTriggerEnter(Collider c)
    {
        if(!ticking){
            am.PlayThought(threeMin);
            init = Time.time;
            ticking = true;
            am.AdvanceTrack(false);
        }
    }
}
