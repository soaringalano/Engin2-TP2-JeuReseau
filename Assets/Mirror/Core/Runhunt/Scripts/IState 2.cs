using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    //execute while initializing the character
    public void OnStart();

    //execute while enter current state
    public void OnEnter();

    //execute while updating the state machine
    public void OnUpdate();

    //execute while fixed updating the state machine
    public void OnFixedUpdate();

    //execute while exiting current state
    public void OnExit();

    //determine if the state machine can enter current state
    public bool CanEnter(IState currentState);

    //determine if the state machine can exit current state
    public bool CanExit();

}
