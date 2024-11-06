using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public enum HunterState
    {
        Leave,
        Attack,
        Trap
    }

    public HunterState _currentState = HunterState.Attack;

    public float attackSpeed;
    public float speed;
    public GameObject garlic;

    private GameObject vampire;
    private Rigidbody2D rb;
    private Vector3 hereToVampire;
    private Vector3 startPos;
    private Camera cam;
    private Vector2 leftBottom;
    private Vector2 leftTop;
    private Vector2 rightTop;
    private Vector3 rightBottom;

    // Start is called before the first frame update
    void Start()
    {
        vampire = GameManager.reference.vampire;
        rb = GetComponent<Rigidbody2D>();
        StartState(_currentState);
        startPos = this.transform.position;
        cam = Camera.main;
        //leftBottom = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        leftTop = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        //rightTop = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        rightBottom = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));
        garlic = GameObject.Find("garlic");
    }

    // Update is called once per frame
    void Update()
    {
        if (vampire != null)
        {
            hereToVampire = vampire.transform.position - this.transform.position;
            hereToVampire = hereToVampire.normalized;
        }
        UpdateState();
    }

    public void StartState(HunterState newState)
    {
        //First run the endstate of our old state
        EndState(_currentState);

        //Run any code that should run once at
        //the beginning of a new state
        switch (newState)
        {
            case HunterState.Attack:
                break;
            case HunterState.Trap:
                print("move to garlic: " + garlic.transform.position);
                Vector3 hereToGarlic = garlic.transform.position - this.transform.position;
                hereToGarlic = hereToGarlic.normalized;
                rb.AddForce(hereToGarlic * speed, ForceMode2D.Impulse);
                break;
            case HunterState.Leave:
                Vector3 hereToCorner = rightBottom - this.transform.position;
                hereToCorner = hereToCorner.normalized;
                rb.AddForce(hereToCorner * speed, ForceMode2D.Impulse);
                break;
            
        }

        _currentState = newState;
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case HunterState.Leave:
                if(this.transform.position.y < rightBottom.y || this.transform.position.x > rightBottom.x)
                {
                    Destroy(this.gameObject);
                }
                break;
            case HunterState.Attack:
                if(GameManager.reference.vampire == null)
                {
                    StartState(HunterState.Trap);
                    break;
                }
                //vampire = GameManager.reference.vampire;
                //Vector3 hereToVampire = vampire.transform.position - this.transform.position;
                //hereToVampire = hereToVampire.normalized;
                attackSpeed = Random.Range(2f, 2f);
                rb.AddForce(hereToVampire * attackSpeed, ForceMode2D.Force);
                if(this.transform.position.y > leftTop.y || this.transform.position.y < rightBottom.y
                    || this.transform.position.x > rightBottom.x || this.transform.position.x < leftTop.x)
                {
                    rb.velocity = Vector3.zero;
                }
                //this.transform.position = Vector3.MoveTowards(this.transform.position, vampire.transform.position, attackSpeed);
                //this.transform.position = Vector3.Lerp(startPos, vampire.transform.position, 1);
                break;
            case HunterState.Trap:
                //if(garlic.transform.position.x - this.transform.position.x < 0.3 &&
                //    garlic.transform.position.y - this.transform.position.y < 0.3)
                //{
                //    garlic.GetComponent<Garlic>().active = true;
                //    StartState(HunterState.Leave);
                //}
                break;
        }
    }
    private void EndState(HunterState oldState)
    {
        //Stop anything that might have been looping,
        //clean up loose ends from whatever state needs it
        switch (oldState)
        {
            case HunterState.Leave:
                break;
            case HunterState.Attack:
                rb.velocity = Vector2.zero;
                break;
            case HunterState.Trap:
                rb.velocity = Vector2.zero;
                break;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject == vampire)
    //    {
    //        float randy = Random.Range(0f, 1f);
    //        if(randy > 0.5f)
    //        {
    //            Destroy(vampire);
    //        }
    //        else
    //        {
    //            Destroy(this.gameObject);
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == vampire)
        {
            float randy = Random.Range(0f, 1f);
            if (randy < 0.7f)
            {
                Destroy(vampire);
                StartState(HunterState.Trap);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        if(collision.gameObject == garlic && _currentState == HunterState.Trap)
        {
            if(!garlic.GetComponent<Garlic>().active)
            {
                garlic.GetComponent<Garlic>().active = true;
                garlic.GetComponent<Garlic>().Invoke("Disintegrate", 20);
            }
            StartState(HunterState.Leave);
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    print("collide");
    //    if(_currentState == HunterState.Trap && collision.gameObject == garlic)
    //    {
    //        garlic.GetComponent<Garlic>().active = true;

    //    }

    //}
}
