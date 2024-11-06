using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject thiefPrefab;
    public GameObject vampirePrefab;
    public GameObject hunterPrefab;
    public static GameManager reference;
    public GameObject thief;
    public GameObject vampire;
    public GameObject hunter;
    public bool vampChasing;

    private Vector3 thiefPos;
    private Vector3 vampPos;
    private void Awake()
    {
        reference = this;
        thief = GameObject.Find("thief");
        vampire = GameObject.Find("vampire");
    }
    // Start is called before the first frame update
    void Start()
    {
        thiefPos = thief.transform.position;
        vampPos = vampire.transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
        if(thief == null)
        {
            float randy = Random.Range(0f, 1f);
            //print(randy);
            if(randy < 0.001f)
            {
                thief = GameObject.Instantiate(thiefPrefab, thiefPos, Quaternion.identity);
            }
        }
        if(vampire != null)
        {
            if (vampire.GetComponent<Vampire>()._currentState == Vampire.VampireState.Chase && !vampire.GetComponent<Vampire>().thiefGone && hunter == null)
            {
                hunter = GameObject.Instantiate(hunterPrefab, new Vector3(3.5f, -3.5f, 0), Quaternion.identity);
            }
        }
        else
        {
            if(hunter == null)
            {
                float randy = Random.Range(0f, 1f);
                print(randy);
                if (randy < 0.003f)
                {
                    vampire = GameObject.Instantiate(vampirePrefab, vampPos, Quaternion.identity);
                }
            }
        }
    }
}
