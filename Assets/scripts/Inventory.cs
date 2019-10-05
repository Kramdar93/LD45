using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private Camera cam;
    private GameObject holdPoint;
    private int currentIndex = 0;
    private int DEFAULT_LAYER, UI_LAYER;
        
    private List<GameObject> stuff;

    public GameObject[] initStuff;

	// Use this for initialization
    void Start()
    {
        cam = transform.GetComponentInChildren<Camera>();
        holdPoint = transform.Find("Main Camera").Find("inventoryPoint").gameObject;
        stuff = new List<GameObject>();
        stuff.Add(null); // don't remove this one.
        Debug.Log("inv size: " + stuff.Count);

        DEFAULT_LAYER = LayerMask.NameToLayer("Default");
        UI_LAYER = LayerMask.NameToLayer("UI");

        //make sure to add all initially.
        foreach(GameObject go in initStuff){
            addObject(go);
        }
        selectNth(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addObject(GameObject go)
    {
        Rigidbody otherRB = go.GetComponent<Rigidbody>();
        if (otherRB != null)
        {
            otherRB.isKinematic = true;
        }
        go.layer = UI_LAYER;
        go.transform.SetParent(holdPoint.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.LookAt(cam.transform, cam.transform.up);
        go.transform.localScale = Vector3.zero;
        stuff.Add(go);
        selectNth(stuff.Count-1);
    }

    public void selectNext()
    {
        selectNth(currentIndex + 1);
    }

    public void selectPrev()
    {
        selectNth(currentIndex - 1);
    }

    private void selectNth(int i)
    {
        if (i >= stuff.Count)
        {
            i = 0;
        }
        else if (i < 0)
        {
            i = stuff.Count - 1;
        }
        Debug.Log("previous: " + currentIndex + "selecting: " + i);
        GameObject current = stuff[currentIndex];
        if (current != null)
        {
            current.transform.localScale = Vector3.zero;
        }
        GameObject next = stuff[i];
        if (next != null)
        {
            next.transform.localScale = Vector3.one;
        }
        currentIndex = i;
    }

    public void dropCurrent()
    {
        GameObject current = stuff[currentIndex];
        if (current != null)
        {
            current.layer = DEFAULT_LAYER;
            current.transform.SetParent(null);
            current.transform.localScale = Vector3.one;
            Rigidbody rb = current.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            stuff.RemoveAt(currentIndex);
            if (currentIndex >= stuff.Count)
            {
                currentIndex = 0;
            }
        }
    }

    public void deleteCurrent()
    {
        GameObject current = stuff[currentIndex];
        if (current != null)
        {
            stuff.RemoveAt(currentIndex);
            if (currentIndex >= stuff.Count)
            {
                currentIndex = 0;
            }
            GameObject.Destroy(current);
        }
    }

    public GameObject getCurrent()
    {
        return stuff[currentIndex];
    }
}
