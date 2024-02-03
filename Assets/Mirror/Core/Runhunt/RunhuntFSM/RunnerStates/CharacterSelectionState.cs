
using UnityEngine;

namespace Mirror
{
    public class CharacterSelectionState : RunnerState
    {

        public override bool CanEnter(IState currentState)
        {
            //if (!m_stateMachine.m_isInCharacterSelectionMenu) Debug.Log("Cannot Enter CharacterSelectionState");
            return m_stateMachine.m_isInCharacterSelectionMenu;
        }

        public override bool CanExit()
        {
            return m_stateMachine.m_isInCharacterSelectionMenu;
        }

        public override void OnEnter()
        {
            Debug.Log("OnEnter() CharacterSelectionState");
            m_stateMachine.RB.constraints = RigidbodyConstraints.FreezePosition;
        }

        public override void OnExit()
        {
            Debug.Log("OnExit() CharacterSelectionState");
            m_stateMachine.RB.constraints = RigidbodyConstraints.None;
        }

        public override void OnStart()
        {
        }

        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
        }

    }
}