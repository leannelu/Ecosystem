using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using DG.Tweening;

public class Vampire : MonoBehaviour
{
    public enum VampireState
    {
        Patrol,
        Sleep,
        Chase
    }

    public VampireState _currentState = VampireState.Patrol;

    private bool sleep;
    public float xspeed;
    public float yspeed;
    public float returnSpeed;
    public float chaseSpeed;
    public Sprite neutral;
    public Sprite attack;
    private GameObject coffin;
    private Coffin coffinScript;
    private Rigidbody2D rb;
    private int facing = -1; //-1 is left
    private SpriteRenderer sprRender;
    private GameObject treasure;
    private GameObject thief;
    private Thief thiefScript;
    public bool thiefGone;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sprRender = this.GetComponent<SpriteRenderer>();
        StartState(_currentState);
        coffin = GameObject.Find("coffin");
        sleep = false;
        coffinScript = coffin.GetComponent<Coffin>();
        treasure = GameObject.Find("treasure");
        neutral = sprRender.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        yspeed = Random.Range(-0.4f, 0.4f);
    }

    public void StartState(VampireState newState)
    {
        //First run the endstate of our old state
        EndState(_currentState);

        //Run any code that should run once at
        //the beginning of a new state
        switch (newState)
        {
            case VampireState.Patrol:
                rb.AddForce(Vector2.right * facing * xspeed, ForceMode2D.Impulse);
                rb.AddForce(Vector2.up * facing * yspeed, ForceMode2D.Impulse);
                InvokeRepeating("Turn", 2, 2);
                Invoke("GoSleep", 10);
                break;
            case VampireState.Sleep:
                Vector3 hereToCoffin = coffin.transform.position - this.transform.position;
                hereToCoffin = hereToCoffin.normalized;
                rb.AddForce(hereToCoffin * xspeed * 2, ForceMode2D.Impulse);
                Invoke("WakeUp", 10);
                break;
            case VampireState.Chase:
                //this.GetComponent<SpriteRenderer>().sprite = attack;
                GameManager.reference.vampChasing = true;
                thiefGone = false;
                break;
        }

        _currentState = newState;
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case VampireState.Patrol:
                if (GameManager.reference.thief != null)
                {
                    thief = GameManager.reference.thief;
                    thiefScript = thief.GetComponent<Thief>();
                    if (thiefScript.visible)
                    {
                        StartState(VampireState.Chase);
                        break;
                    }
                }
                if (sleep)
                {
                    StartState(VampireState.Sleep);
                }
                break;
            case VampireState.Sleep:
                if (coffinScript.inCoffin)
                {
                    rb.velocity = Vector2.zero;
                }
                if (!sleep)
                {
                    StartState(VampireState.Patrol);
                }
                break;
            case VampireState.Chase:
                if (thiefGone)
                {
                    if (coffinScript.inCoffin)
                        StartState(VampireState.Patrol);
                    break;
                }
                Vector3 hereToThief = thief.transform.position - this.transform.position;
                hereToThief = hereToThief.normalized;
                chaseSpeed = Random.Range(1f, 2.5f);
                rb.AddForce(hereToThief * chaseSpeed, ForceMode2D.Force);
                break;
        }
    }
    private void EndState(VampireState oldState)
    {
        //Stop anything that might have been looping,
        //clean up loose ends from whatever state needs it
        switch (oldState)
        {
            case VampireState.Patrol:
                CancelInvoke("Turn");
                CancelInvoke("GoSleep");
                rb.velocity = Vector2.zero;
                break;
            case VampireState.Sleep:
                CancelInvoke("WakeUp");
                facing = -1;
                sprRender.flipX = false;
                rb.AddForce(Vector2.right * facing * xspeed, ForceMode2D.Impulse);
                sleep = false;
                coffinScript.inCoffin = false;
                break;
            case VampireState.Chase:
                rb.velocity = Vector2.zero;
                facing = -1;
                sprRender.flipX = false;
                rb.AddForce(Vector2.right * facing * xspeed, ForceMode2D.Impulse);
                coffinScript.inCoffin = false;
                GameManager.reference.vampChasing = false;
                break;
        }
    }

    private void Turn()
    {
        rb.velocity = Vector2.zero;
        facing *= -1;
        sprRender.flipX = !sprRender.flipX;
        rb.AddForce(Vector2.right * facing * xspeed, ForceMode2D.Impulse);
        rb.AddForce(Vector2.up * facing * yspeed, ForceMode2D.Impulse);
    }
    private void GoSleep()
    {
        sleep = true;
    }

    private void WakeUp()
    {
        sleep = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == thief)
        {
            GameObject.Destroy(thief);
            thief = null;
        }
        Return();
    }
    public void Return()
    {
        //sprRender.sprite = neutral;
        thiefGone = true;
        rb.velocity = Vector2.zero;
        Vector3 hereToCoffin = coffin.transform.position - this.transform.position;
        hereToCoffin = hereToCoffin.normalized;
        rb.AddForce(hereToCoffin * returnSpeed, ForceMode2D.Impulse);
    }
}
