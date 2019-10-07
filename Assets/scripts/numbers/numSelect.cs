using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class numSelect : MonoBehaviour {

    public Sprite[] font;

    private int current = 0;
    private SpriteRenderer display;

	// Use this for initialization
	void Start () {
        display = transform.Find("disp").GetComponent<SpriteRenderer>();
	}

    public void selectNumber(int i)
    {
        current = i;
        if (current > 9)
        {
            current = 0;
        }
        else if (current < 0)
        {
            current = 9;
        }

        display.sprite = font[current];
    }

    public void Increment()
    {
        selectNumber(current + 1);
    }

    public void Decrement()
    {
        selectNumber(current - 1);
    }

    public int getDigit()
    {
        return current;
    }
}
