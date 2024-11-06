using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager reference;
    public GameObject thief;
    public GameObject vampire;
    public GameObject hunter;

    private void Awake()
    {
        reference = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
