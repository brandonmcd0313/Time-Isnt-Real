using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    // Use this for initialization
    Rigidbody2D rb2d; //reference to Rigidbody2D component
    public int speed = 1;
    bool canJump; public float jumpForce = 450f;
    public GameObject j1, j2; //jump check objects
    //grabbing objects
    public GameObject gd1,gd2, grabbed, grabDetect, rewind;
    public GameObject cage;
    bool isGrabbing; bool ran = false;
    public Text countdown; bool canMove = true;
    //movement storages
    List<float> nums = new List<float>();
    List<float> instans = new List<float>();
    float tempX; bool isRec = false;
    float tempY; bool doPlay = false;
    float tempZ; public ParticleSystem breaker;
    SpriteRenderer player; Animator anim;
    public Renderer Rerend; ParticleSystem ps;
   public ParticleSystem win; ParticleSystem p3;
    public AudioSource explode, jump, pickup, plate, taperewind, won, drop;
    Collider2D col;
    void Start()
    {
        anim = GetComponent<Animator>();
        //set the resolution of the window
        Screen.SetResolution(1024, 768, false);
        StartCoroutine(Countdown());
        player = GetComponent<SpriteRenderer>();
        Rerend = rewind.GetComponent<Renderer>();
        Rerend.enabled = false;
        col = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>(); //hook up rb2d
        if (breaker != null)
        {
            ps = breaker.GetComponent<ParticleSystem>();
        }
        p3 = win.GetComponent<ParticleSystem>();
        anim.StopPlayback();
    }


    // Update is called once per frame
    void Update()
    {
        anim.StartPlayback();
        print(canJump);
        //locking rotation
        transform.eulerAngles = new Vector3(0, 0, 0);
        if (canMove)
        {
            //left/right movement
            if (Input.GetKey(KeyCode.D)) //right
            {
                anim.StopPlayback();
                //Time.deltaTime is the amount of time between
                // one frame and the next, used for smoothing
                transform.position +=
                    new Vector3(speed * Time.deltaTime, 0);

            }
            if (Input.GetKey(KeyCode.A)) //left
            {
                anim.StopPlayback();
                transform.position -=
                    new Vector3(speed * Time.deltaTime, 0);
            }
            //jump
            canJump = false;
            canJump = false;
            //check if there is a platform beneath the player
            if (Physics2D.OverlapArea(j1.transform.position,
                j2.transform.position) != null)
            {
                canJump = true;
            }

            //the moment they press the space bar, apply up force
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                anim.StartPlayback();
                rb2d.AddForce(new Vector3(0, jumpForce));
                jump.PlayOneShot(jump.clip, 1);
            }

            if (Physics2D.OverlapArea(gd1.transform.position,
                gd2.transform.position) != null) //in grab zone
                {
                print("found it");
                //store what we detect
                grabDetect = Physics2D.OverlapArea(gd1.transform.position, gd2.transform.position).gameObject;
                //interacting
                if (Input.GetKeyDown(KeyCode.E))
                {
                        print("run it!");
                        GrabDrop();
                        return;
                   
                }
            }

            //make grapped object move with player
            if(isGrabbing)
            {
                grabbed.transform.localPosition = new Vector3(3.88f, 0, 0);
            }

            //dropping a held object
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isGrabbing)
                {
                    GrabDrop();
                    return;
                }
            }


            }
            if (isRec == true)
            {
                tempX = transform.position.x;
                tempY = transform.position.y;
                tempZ = transform.position.z;
                nums.Add(tempX);
                nums.Add(tempY);
                nums.Add(tempZ);
            }
        
            if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator Countdown()
    {

        //10 second countdown
        for (int i = 10; i >= 0; i--)
        {
            countdown.text = "Time Remaining : " + i;
           
            if (i == 5)
            {
                isRec = true;
            }
            yield return new WaitForSeconds(1f);
        }

        //call the reversal
        StartCoroutine(Playback());
        //reset recorder
        isRec = false;
    }

    public IEnumerator Playback()
    {
        player.color = Color.green;
        Rerend.enabled = true;
        taperewind.PlayOneShot(taperewind.clip, 1);
        col.enabled = false;
        //if holdin gsomething
        if (isGrabbing)
        {
            GrabDrop();
        }
        rewind.GetComponent<Animator>().Play("rewind", -1, 0);
        for (int i = nums.Count-1; i >= 0; i -= 3)
        {
            transform.position = new Vector3(nums[i - 2], nums[i - 1], nums[i]);
            
            yield return null;
        }
        col.enabled = true;
        Rerend.enabled = false;
        nums.Clear();
        canMove = true;
        player.color = Color.white;
        StartCoroutine(Countdown());
    }

    public void GrabDrop()
    {
        //Check if we do have a grabbed object => drop it
        if (isGrabbing)
        {
            drop.PlayOneShot(drop.clip, 1);
            //make isGrabbing false
            isGrabbing = false;
            //unparent the grabbed object
            grabbed.transform.parent = null;
            //radd gravity so object falls to floor
            grabbed.GetComponent<Rigidbody2D>().gravityScale = 10.0f;
            grabbed.GetComponent<Rigidbody2D>().mass = 100.0f;
            grabbed.GetComponent<locking>().lockObject();
            //lock the object
            //make it red
            grabbed.GetComponent<SpriteRenderer>().color = Color.red;
            grabbed = null;
        }
        //Check if we have nothing grabbed grab the detected item
        else
        {
            if (grabDetect.GetComponent<locking>() != null)
            {
                if (grabDetect.GetComponent<locking>().checkLock() == false)
                {
                    pickup.PlayOneShot(pickup.clip, 1);
                    //Enable the isGrabbing bool
                    isGrabbing = true;
                    //assign the grabbed object to the object itself
                    grabbed = grabDetect;
                    //Parent the grabbed object to the player
                    grabbed.transform.parent = transform;
                    //Adjust the position of the grabbed object to be closer to hands      
                    //set gravity scale to 0 for detected
                    grabbed.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                    grabbed.transform.localPosition = new Vector3(3.57f, 0, 0);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       

        if (other.name == "padtaglev2" && !ran ) //level 2 plate
        {
            //its a game jam i can have spagetti code
            plate.PlayOneShot(plate.clip, 1);
            Destroy(cage);
            explode.PlayOneShot(explode.clip, 1);
            ps.Play();
            ran = true;

        }

        if (other.name == "platedetect3" && !ran) //level 2 plate
        {
            ran = true;
            //its a game jam i can have spagetti code
            //this is the stopper for trapdoor
            plate.PlayOneShot(plate.clip, 1);
            Destroy(cage);
            explode.PlayOneShot(explode.clip, 1);
            ps.Play();

        }

        if (other.tag == "Finish")
        {
            //win particles

            //win sound

            //load next scene
            StopAllCoroutines();
            StartCoroutine(Dowin());
        }
    }

    IEnumerator Dowin()
    {

        canMove = false;
        won.PlayOneShot(won.clip, 1);
        p3.Play();
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1));
    }
}
