
using UnityEngine;

namespace Mirror
{
    public class CharacterSelectionState : RunnerState
    {
        public override bool CanEnter(IState currentState)
        {
            return m_stateMachine.GetComponentInParent<CanvasReferencer>().sceneReferencer.characterSelectionObject.activeSelf;
        }

        public override bool CanExit()
        {
            return !m_stateMachine.GetComponentInParent<CanvasReferencer>().sceneReferencer.characterSelectionObject.activeSelf;
        }

        public override void OnEnter()
        {
            Debug.Log("CharacterSelectionState OnEnter()");
            m_stateMachine.RunnerUI.SetActive(false);
        }

        public override void OnExit()
        {
            Debug.Log("CharacterSelectionState OnExit()");
            m_stateMachine.RunnerUI.SetActive(true);
        }

        public override void OnStart()
        {
            //base.OnStart();
        }

        public override void OnUpdate()
        {
            //base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
        }
    }
}