using Runhunt.FSM;
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
            return m_stateMachine.m_gameEnded ||
                m_stateMachine.m_winnedRunner == m_stateMachine.m_runners.Count;
        }

        public override bool CanExit()
        {
            return true;
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

