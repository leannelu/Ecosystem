using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffin : MonoBehaviour
{
    public bool inCoffin;
    // Start is called before the first frame update
    void Start()
    {
        inCoffin = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vampire vampireScript = collision.gameObject.GetComponent<Vampire>();
        if(vampireScript != null)
        {
            inCoffin = true;
        }
    }
}
