using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observation : MonoBehaviour {

    public AudioClip line;
    public string id;
    public bool destroyOnDone;

    private AudioManager am;

    void Start()
    {
        myInit();
    }

    public void myInit()
    {
        am = Component.FindObjectOfType<AudioManager>();
    }

    public virtual void Observe()
    {
        if (line != null)
        {
            Debug.Log("playing: " + line.name);
            am.PlayThought(line);
        }

        if (destroyOnDone)
        {
            Destroy(this);
        }
    }
}
