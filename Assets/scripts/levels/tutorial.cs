using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour {

    public bool observedWorld = false;
    public bool observedNote = false;

    public void checkedNote()
    {
        observedNote = true;
        checkConditions();
    }
    public void checkedWorld()
    {
        Debug.Log("saw world");
        observedWorld = true;
        checkConditions();
    }
    private void checkConditions()
    {
        if (observedWorld && observedNote)
        {
            Debug.Log("tut done");
            Trapped trapBehavior = Component.FindObjectOfType<Trapped>();
            trapBehavior.isActive = false;

            //janky hack m8
            AudioSource src = Component.FindObjectOfType<leavePurgatory>().transform.parent.GetComponent<AudioSource>();
            src.Play();
        }
        else
        {
            Debug.Log("tut in progress");
        }
    }
}
