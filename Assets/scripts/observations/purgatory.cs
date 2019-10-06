using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class purgatory : Observation {

    private tutorial tut;

    void Start()
    {
        tut = Component.FindObjectOfType<tutorial>();

        // init base since Start is protected.
        myInit();
    }

    public override void Observe()
    {
        Debug.Log("saw ground");

        //advance objective
        tut.checkedWorld();

        // pass through
        base.Observe();
    }
}
