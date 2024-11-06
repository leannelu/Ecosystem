using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Thief : MonoBehaviour
{
    public enum ThiefState
    {
        Hide,
        Steal,
        Flee
    }

    private ThiefState _currentState = ThiefState.Hide;

    public float xspeed;
    public float yspeed;
    public bool visible;

    private Rigidbody2D rb;
    private SpriteRenderer sprRender;
    private int facing = 1; //-1 is left, 1 is right
    private Coffin coffin;
    private GameObject treasure;
    private Camera cam;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sprRender = this.GetComponent<SpriteRenderer>();
        StartState(_currentState);
        GameObject coffinObj = GameObject.Find("coffin");
        coffin = coffinObj.GetComponent<Coffin>();
        treasure = GameObject.Find("treasure");
        cam = Camera.main;
        visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    public void StartState(ThiefState newState)
    {
        //First run the endstate of our old state
        EndState(_currentState);

        //Run any code that should run once at
        //the beginning of a new state
        switch (newState)
        {
            case ThiefState.Hide:
                rb.AddForce(Vector2.right * facing * xspeed, ForceMode2D.Impulse);
                InvokeRepeating("Turn", 1.5f, 1.5f);
                visible = false;
                break;
            case ThiefState.Steal:
                visible = true;
                Vector3 hereToTreasure = treasure.transform.position - this.transform.position;
                hereToTreasure = hereToTreasure.normalized;
                facing = 1;
                sprRender.flipX = false;
                xspeed = Random.Range(0.8f, 1.4f);
                print("thief steal speed: " + xspeed);
                rb.AddForce(hereToTreasure * xspeed, ForceMode2D.Impulse);
                break;
            case ThiefState.Flee:
                xspeed = Random.Range(1f, 1.8f);
                print("thief flee speed: " + xspeed);
                rb.AddForce(Vector2.down * xspeed, ForceMode2D.Impulse);
                break;
        }

        _currentState = newState;
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case ThiefState.Hide:
                if(coffin.inCoffin)
                {
                    StartState(ThiefState.Steal);
                }
                break;
            case ThiefState.Steal:
                if(treasure.transform.position.x - this.transform.position.x < 1 &&
                    treasure.transform.position.y - this.transform.position.y < 1)
                {
                    StartState(ThiefState.Flee);
                }
                break;
            case ThiefState.Flee:
                Vector3 bottomEdge = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
                if(this.transform.position.y < bottomEdge.y)
                {
                    if(GameManager.reference.vampire != null)
                        GameManager.reference.vampire.GetComponent<Vampire>().Return();
                    Destroy(this.gameObject);
                }
                break;
        }
    }
    private void EndState(ThiefState oldState)
    {
        //Stop anything that might have been looping,
        //clean up loose ends from whatever state needs it
        switch (oldState)
        {
            case ThiefState.Hide:
                rb.velocity = Vector2.zero;
                CancelInvoke("Turn");
                break;
            case ThiefState.Steal:
                rb.velocity = Vector2.zero;
                break;
            case ThiefState.Flee:
                break;
        }
    }

    private void Turn()
    {
        rb.velocity = Vector2.zero;
        facing *= -1;
        sprRender.flipX = !sprRender.flipX;
        rb.AddForce(Vector2.right * facing * xspeed, ForceMode2D.Impulse);
    }
}
