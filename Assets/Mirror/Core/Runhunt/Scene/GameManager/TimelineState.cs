using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{

    public class TimelineState : GameState
    {
        public TimelineState(GameManagerFSM stateMachine) : base(stateMachine)
        {
        }

        public override bool CanEnter(IState currentState)
        {
            return false;
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