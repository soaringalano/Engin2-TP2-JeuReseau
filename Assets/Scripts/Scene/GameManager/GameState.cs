using Mirror;
using Runhunt.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{

    public class GameState : IState
    {

        protected GameManagerFSM m_stateMachine;

        public GameState(GameManagerFSM stateMachine)
        {
            m_stateMachine = stateMachine;
        }

        public virtual bool CanEnter(IState currentState)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool CanExit()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnStart()
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }

}
