using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class HunterFreeState : HunterState
    {
        public override bool CanEnter(IState currentState)
        {
            return m_stateMachine.GetCurrentDirectionalInput().magnitude > 0;
        }

        public override bool CanExit()
        {
            return true;
        }

        public override void OnEnter()
        {
            Debug.Log("Enter state: HunterFreeState\n");
        }

        public override void OnExit()
        {
            Debug.Log("Exit state: HunterFreeState\n");

        }

        public override void OnStart()
        {
            base.OnStart();
        }

        public override void OnUpdate()
        {
            m_stateMachine.EnableMouseTracking();
            m_stateMachine.SetStopLookAt(true);
            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            m_stateMachine.FixedMoveByDirectionalInput();
        }
    }
}