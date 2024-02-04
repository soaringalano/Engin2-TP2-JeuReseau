
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
                Debug.Log("Scene is null, that can mean the spawn is made in selection.");
                m_sceneRef = m_stateMachine.Scene.GetComponentInChildren<SceneReferencer>();
            }
            else if (m_stateMachine.Scene == null)
            {
                Debug.Log("Scene is null, that can mean the spawn is made in scene.");
                // Source : https://discussions.unity.com/t/find-gameobjects-in-specific-scene-only/163901
                GameObject[] gameObjects = m_stateMachine.gameObject.scene.GetRootGameObjects();
                GameObject sceneGO = null;

                foreach (GameObject _gameObject in gameObjects)
                {
                    if (_gameObject.name != "Scene") continue;

                    sceneGO = _gameObject;
                    break;
                }

                SceneReferencer m_sceneRef = sceneGO.GetComponentInChildren<SceneReferencer>();
                if (m_sceneRef == null) Debug.LogError("SceneReferencer not found in children of Scene!");

                //Debug.Log("CharacterSelectionState OnStart()");
                //GameObject scene = m_stateMachine.gameObject.scene.GetRootGameObjects()[0];
                //if (scene.name != "Scene") Debug.LogError("First child of scene is not Scene GO! Please put Scene as first child of the scene!");

                //SceneReferencer m_sceneRef = scene.GetComponentInChildren<SceneReferencer>();
                //if (m_sceneRef == null) Debug.LogError("SceneReferencer not found in children of Scene!");

                if (m_sceneRef == null) Debug.LogError("m_sceneRef null");
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
            m_stateMachine.RunnerUI.SetActive(false);
        }

        public override void OnExit()
        {
            Debug.Log("CharacterSelectionState OnExit()");
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