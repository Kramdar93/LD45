using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physDoor : Interaction {
    public bool isLocked;
    public AudioClip lockedClip;
    public AudioClip unlockSFX;
    public int requiredKey = -1;

    private AudioManager am;
    private Rigidbody rb;
    private PlayerRBMover player;

    void Start()
    {
        am = GameObject.FindObjectOfType<AudioManager>();
        rb = transform.parent.GetComponent<Rigidbody>();
        player = GameObject.FindObjectOfType<PlayerRBMover>();
    }

    public override void doAction()
    {
        if (isLocked)
        {
            tryUnlock();
            if (isLocked && lockedClip != null)
            {
                am.PlayThought(lockedClip);
            }
        }
        else if (rb.isKinematic)
        {
            am.PlayAt(unlockSFX, transform.position);
            rb.isKinematic = false;
        }
    }

    public void tryUnlock()
    {
        if (requiredKey >= 0)
        {
            Inventory inv = player.transform.GetComponent<Inventory>();
            GameObject held = inv.getCurrent();
            if(held == null){return;}
            Key k = held.GetComponent<Key>();
            if (k != null && k.id == requiredKey)
            {
                isLocked = false;
                am.PlayAt(unlockSFX, transform.position);
                rb.isKinematic = false;
            }
        }
    }
}
