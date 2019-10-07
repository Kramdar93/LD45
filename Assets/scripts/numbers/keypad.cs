using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keypad : MonoBehaviour {

    public int[] code;

    public GameObject[] digits;

    public AudioClip opened;

    private AudioManager am;
    private bool done = false;

    private List<numSelect> nums;

    void Start()
    {
        am = GameObject.FindObjectOfType<AudioManager>();
        nums = new List<numSelect>();
        foreach (GameObject g in digits)
        {
            numSelect ns = g.GetComponent<numSelect>();
            if (ns != null)
            {
                nums.Add(ns);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        tryCombo(); // call this on update
	}

    public bool tryCombo()
    {
        if (!done)
        {
            Debug.Log("trying");
            string s = "";
            bool correct = true;
            for (int i = 0; i < code.Length && i < nums.Count; ++i)
            {
                s += nums[i].getDigit();
                if (code[i] != nums[i].getDigit())
                {
                    correct = false;
                    break;
                }
            }
            Debug.Log(s);
            if (correct)
            {
                done = true;
                Debug.Log("opened");
                if (opened != null)
                {
                    am.PlayAt(opened, transform.position);
                }
                return true;
            }
        }
        return false;
    }

    public bool getDone()
    {
        return done;
    }
}
