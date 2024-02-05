
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    public class CharacterSelectionState : RunnerState
    {
        private SceneReferencer m_sceneRef;

        public override void OnStart()
        {
            Debug.Log("CharacterSelectionState OnStart(): " + m_stateMachine.Scene.name);
            if (m_stateMachine.Scene != null)
            {
                Debug.Log("Scene is not null, that can mean the spawn is made in selection: " + m_stateMachine.Scene.name);
                m_sceneRef = m_stateMachine.Scene.gameObject.GetComponentInChildren<SceneReferencer>();
                if (m_sceneRef == null) Debug.LogError("SceneReferencer not found in children of scene!");
            }
            else if (m_stateMachine.Scene == null)
            {
                GameObject sceneGO = m_stateMachine.GetScene();
                m_sceneRef = sceneGO.GetComponentInChildren<SceneReferencer>();

                if (m_sceneRef == null) Debug.LogError("SceneReferencer not found in children of Scene!");
                if (m_sceneRef.characterSelectionObject == null) Debug.LogError("characterSelectionObject null");
            }

            base.OnStart();
        }

        public override bool CanEnter(IState currentState)
        {
            if (m_sceneRef == null) Debug.LogError("m_sceneRef null");
            if (m_sceneRef.characterSelectionObject == null) Debug.LogError("characterSelectionObject null");

            return m_sceneRef.characterSelectionObject.activeSelf;
        }

        public override bool CanExit()
        {
            if (m_sceneRef == null) Debug.LogError("m_sceneRef null");
            if (m_sceneRef.characterSelectionObject == null) Debug.LogError("characterSelectionObject null");
            return !m_sceneRef.characterSelectionObject.activeSelf;
        }

        public override void OnEnter()
        {
            Debug.Log("CharacterSelectionState OnEnter()");
            m_stateMachine.UI.SetActive(false);
        }

        public override void OnExit()
        {
            Debug.Log("CharacterSelectionState OnExit()");
            m_stateMachine.UI.SetActive(true);
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