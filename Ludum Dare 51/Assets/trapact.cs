using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapact : MonoBehaviour {
    public GameObject trap;
    // Use this for initialization
    public ParticleSystem button, trapper;
    public AudioSource explode, plate;
    ParticleSystem ps,ps2; int runs = 0;
	void Start () {
        ps = button.GetComponent<ParticleSystem>();
        ps2 = trapper.GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {

    }//thebutton

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "thebutton")
        {
            plate.PlayOneShot(plate.clip, 1);

            StartCoroutine(onButton());
        }

        
    }

    IEnumerator onButton()
    {
        if (runs == 0)
        {
            ps.Play(); //for button
            yield return new WaitForSeconds(0.5f);
            Destroy(trap);
            explode.PlayOneShot(explode.clip, 1);
            ps2.Play();
        }
        runs++;

    }
}
