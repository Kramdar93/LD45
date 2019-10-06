using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leavePurgatory : Interaction {

    public override void doAction()
    {
        transform.parent.GetComponent<AudioSource>().Stop();

        //todo: transit level
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("outskirts");
    }
}
