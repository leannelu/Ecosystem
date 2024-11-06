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

    private VampireState _currentState = VampireState.Patrol;

    private bool sleep;
    public float xspeed;
    public float yspeed;
    private GameObject coffin;
    private Coffin coffinScript;
    private Rigidbody2D rb;
    private int facing = -1; //-1 is left
    private SpriteRenderer sprRender;
    private GameObject treasure;
    private GameObject thief;
    private Thief thiefScript;

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
                rb.AddForce(hereToCoffin * xspeed, ForceMode2D.Impulse);
                Invoke("WakeUp", 10);
                break;
            case VampireState.Chase:

                break;
        }

        _currentState = newState;
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case VampireState.Patrol:

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
                //if(thiefSteal)
                //{

                //}
                if(!sleep)
                {
                    StartState(VampireState.Patrol);
                }
                break;
            case VampireState.Chase:

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
        print("sleep");
        sleep = true;
    }

    private void WakeUp()
    {
        sleep = false;
    }
    
}
