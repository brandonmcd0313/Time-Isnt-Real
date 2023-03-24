using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.SetResolution(1024, 768, false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void beginGame()
    {
        Screen.SetResolution(1024, 768, false);
        SceneManager.LoadScene(2);
    }

    public void instruct()
    {
        Screen.SetResolution(1024, 768, false);
        SceneManager.LoadScene(1);
    }
}
