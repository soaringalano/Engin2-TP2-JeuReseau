using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class GameStartState : GameState
    {
        public GameStartState(GameManagerFSM stateMachine) : base(stateMachine)
        {
        }

        public override bool CanEnter(IState currentState)
        {
            return true;
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
