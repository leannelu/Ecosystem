using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public enum ThiefState
    {
        Hide,
        Steal,
        Flee
    }

    private ThiefState _currentState = ThiefState.Hide;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
                break;
            case ThiefState.Steal:
                break;
            case ThiefState.Flee:

                break;
        }

        _currentState = newState;
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case ThiefState.Hide:
                break;
            case ThiefState.Steal:
                break;
            case ThiefState.Flee:

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
                break;
            case ThiefState.Steal:
                break;
            case ThiefState.Flee:

                break;
        }
    }
}
