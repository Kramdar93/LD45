using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorLock : keypad {

    public GameObject[] doors;

    void Update()
    {
        if (base.tryCombo())
        {
            Open();
        }
    }

    void Open()
    {
        foreach (GameObject g in doors)
        {
            physDoor pdoor = g.GetComponentInChildren<physDoor>();
            levelDoor ldoor = g.GetComponentInChildren<levelDoor>();

            if (pdoor != null)
            {
                pdoor.isLocked = false;
            }

            if (ldoor != null)
            {
                ldoor.isLocked = false;
            }
        }
    }
}
