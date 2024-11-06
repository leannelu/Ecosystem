using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garlic : MonoBehaviour
{

    public bool active;
    private SpriteRenderer sprRenderer;
    // Start is called before the first frame update
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            sprRenderer.enabled = true;

        }
        else
        {
            sprRenderer.enabled = false;
        }
    }

    public void Disintegrate()
    {
        active = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    //private void onTriggerEnter2D(Collision2D collision)
    //{

    //    if(collision.gameObject == GameManager.reference.vampire && active)
    //    {
    //        GameObject vampire = GameManager.reference.vampire;
    //        float randy = Random.Range(0f, 1f);
    //        //play hiss
    //        if(randy < 0.3f)
    //        {
    //            Destroy(vampire);
    //        }
    //        Invoke("Disintegrate", 10);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.reference.vampire && active)
        {
            GameObject vampire = GameManager.reference.vampire;
            float randy = Random.Range(0f, 1f);
            //play hiss
            print("ow!");
            if (randy < 0.4f)
            {
                Destroy(vampire);
            }
        }
        //else if(collision.gameObject == GameManager.reference.hunter)
        //{
        //    Hunter hunterScript = GameManager.reference.hunter.GetComponent<Hunter>();
        //    if (hunterScript._currentState == Hunter.HunterState.Trap)
        //    {
        //        active = true;
        //    }

        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.reference.vampire && active)
        {
            GameObject vampire = GameManager.reference.vampire;
            float randy = Random.Range(0f, 1f);
            //play hiss
            print("ow!");
            if (randy < 0.3f)
            {
                Destroy(vampire);
            }
        }
    }
}
