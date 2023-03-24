using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class locking : MonoBehaviour {
    bool isLocked = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void lockObject()
    {
        isLocked = true;
    }

    public bool checkLock()
    {
        return isLocked;
    }
}
