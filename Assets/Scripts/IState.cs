using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    /**
     * Invoked in Start method
     * execute while initializing the character
     */
    public void OnStart();

    /**
     * Invoked in Update method
     * execute while enter current state
     */
    public void OnEnter();

    /**
     * Invoked in Update method
     * execute while updating the state machine
     */
    public void OnUpdate();

    /**
     * Invoked in FixedUpdate method
     * execute while fixed updating the state machine
     */
    public void OnFixedUpdate();

    /**
     * Invoked in Update method
     * execute while exiting current state
     */
    public void OnExit();

    /**
     * Invoked in Update method
     * determine if the state machine can enter current state
     */
    public bool CanEnter(IState currentState);

    /**
     * Invoked in Update method
     * determine if the state machine can exit current state
     */
    public bool CanExit();

}
