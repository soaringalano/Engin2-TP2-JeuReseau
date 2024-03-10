using UnityEngine;

namespace Mirror
{
    public class RunnerCharacterSelectionState : RunnerState
    {
        //private SceneReferencer m_sceneRef;

        public override void OnStart()
        {
            Debug.Log("RunnerCharacterSelectionState OnStart(): " + AbstractNetworkFSM<RunnerState>.Scene.name);
            //if (AbstractNetworkFSM<RunnerState>.Scene != null)
            //{
            //    Debug.Log("Scene is not null, that can mean the spawn is made in selection: " + AbstractNetworkFSM<RunnerState>.Scene.name);
            //    m_sceneRef = AbstractNetworkFSM<RunnerState>.Scene.gameObject.GetComponentInChildren<SceneReferencer>();
            //    if (m_sceneRef == null) Debug.LogError("SceneReferencer not found in children of scene!");
            //}
            //else if (AbstractNetworkFSM<RunnerState>.Scene == null)
            //{
            //    GameObject sceneGO = AbstractNetworkFSM<RunnerState>.GetScene(m_stateMachine.gameObject);
            //    m_sceneRef = sceneGO.GetComponentInChildren<SceneReferencer>();

            //    if (m_sceneRef == null) Debug.LogError("SceneReferencer not found in children of Scene!");
            //    if (m_sceneRef.characterSelectionObject == null) Debug.LogError("characterSelectionObject null");
            //}

            base.OnStart();
        }

        public override bool CanEnter(IState currentState)
        {
            if (currentState is RagdollState)
            {
                return false;
            }
            //if (m_sceneRef == null) Debug.LogError("m_sceneRef null");
            //if (m_sceneRef.characterSelectionObject == null) Debug.LogError("characterSelectionObject null");

            return false; // m_sceneRef.characterSelectionObject.activeSelf;
        }

        public override bool CanExit()
        {
            //if (m_sceneRef == null) Debug.LogError("m_sceneRef null");
            //if (m_sceneRef.characterSelectionObject == null) Debug.LogError("characterSelectionObject null");
            return false; // !m_sceneRef.characterSelectionObject.activeSelf;
        }

        public override void OnEnter()
        {
            Debug.Log("RunnerCharacterSelectionState OnEnter()");
            m_stateMachine.RunnerUI.SetActive(false);
        }

        public override void OnExit()
        {
            Debug.Log("RunnerCharacterSelectionState OnExit()");
            m_stateMachine.RunnerUI.SetActive(true);
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