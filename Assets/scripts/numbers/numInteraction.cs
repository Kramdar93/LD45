using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class numInteraction : Interaction {

    private numSelect target;
    public bool positive;

    void Start()
    {
        target = transform.parent.GetComponent<numSelect>();
    }

    public override void doAction()
    {
        if (target == null) { return; }
        if (positive)
        {
            target.Increment();
        }
        else
        {
            target.Decrement();
        }
        base.doAction();
    }
}
