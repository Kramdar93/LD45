using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelDoor : Interaction {

    public string nextSceen;
    public bool isLocked;
    public AudioClip lockedClip;
    public int requiredKey = -1;

    private AudioManager am;
    private PlayerRBMover player;

    void Start()
    {
        am = GameObject.FindObjectOfType<AudioManager>();
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
        else if (nextSceen != null && !nextSceen.Equals(""))
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextSceen);
        }
    }

    public void tryUnlock()
    {
        if (requiredKey >= 0)
        {
            Inventory inv = player.transform.GetComponent<Inventory>();
            GameObject held = inv.getCurrent();
            if (held == null) { return; }
            Key k = held.GetComponent<Key>();
            if (k != null && k.id == requiredKey)
            {
                isLocked = false;
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextSceen);
            }
        }
    }
}
