using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class GameEndState : GameState
    {
        public GameEndState(GameManagerFSM stateMachine) : base(stateMachine)
        {
        }

        public override bool CanEnter(IState currentState)
        {
            return m_stateMachine.m_gameEnded;
        }

        public override bool CanExit()
        {
            return false;
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnStart()
        {
        }

        public override void OnUpdate()
        {
        }

    }

}

