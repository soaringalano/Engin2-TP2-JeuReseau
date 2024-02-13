using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{

    // if hunter wins, then runner loses, but still need to wait for the timer to end
    public class HunterWinState : GameState
    {
        public HunterWinState(GameManagerFSM stateMachine) : base(stateMachine)
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