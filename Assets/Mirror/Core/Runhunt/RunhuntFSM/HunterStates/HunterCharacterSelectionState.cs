﻿using UnityEngine;

namespace Mirror
{
    public class HunterCharacterSelectionState : HunterState
    {
        //private SceneReferencer m_sceneRef;

        public override void OnStart()
        {
            Debug.Log("HunterCharacterSelectionState OnStart(): " + AbstractNetworkFSM<HunterState>.Scene.name);
            //if (AbstractNetworkFSM<HunterState>.Scene != null)
            //{
            //    Debug.Log("Scene is not null, that can mean the spawn is made in selection: " + AbstractNetworkFSM<HunterState>.Scene.name);
            //    m_sceneRef = AbstractNetworkFSM<HunterState>.Scene.gameObject.GetComponentInChildren<SceneReferencer>();
            //    if (m_sceneRef == null) Debug.LogError("SceneReferencer not found in children of scene!");
            //    else Debug.Log("SceneReferencer found in children of scene!");
            //}
            //else if (AbstractNetworkFSM<HunterState>.Scene == null)
            //{
            //    GameObject sceneGO = AbstractNetworkFSM<RunnerState>.GetScene(m_stateMachine.gameObject);
            //    m_sceneRef = sceneGO.GetComponentInChildren<SceneReferencer>();

            //    if (m_sceneRef == null) Debug.LogError("SceneReferencer not found in children of Scene!");
            //    if (m_sceneRef.characterSelectionObject == null) Debug.LogError("characterSelectionObject null!");
            //    else Debug.Log("OnStart() characterSelectionObject not null!");
            //}

            base.OnStart();
        }

        public override bool CanEnter(IState currentState)
        {
            //if (m_sceneRef == null) Debug.LogError("m_sceneRef null");
            //if (m_sceneRef.characterSelectionObject == null) Debug.LogError("characterSelectionObject null");

            return false; // m_sceneRef.characterSelectionObject.activeSelf;
        }

        public override bool CanExit()
        {
            //if (m_sceneRef == null) Debug.LogError("m_sceneRef null");
            //if (m_sceneRef.characterSelectionObject == null) Debug.LogError("characterSelectionObject null");
            //Debug.Log("HunterCharacterSelectionState CanExit(): " + !m_sceneRef.characterSelectionObject.activeSelf);

            return false; // !m_sceneRef.characterSelectionObject.activeSelf;
        }

        public override void OnEnter()
        {
            //if (!m_stateMachine.IsInitialized) return;

            //Debug.Log("HunterCharacterSelectionState OnEnter()");
            //if (m_stateMachine == null) Debug.LogWarning("m_stateMachine is not initialized yet!");
            //if (m_stateMachine.HunterSelectionPose == null) Debug.LogWarning("m_stateMachine.HunterSelectionPose is not initialized yet!");

            // Will make the mode appear in free state : m_stateMachine.HunterSelectionPose.gameObject.SetActive(true); 
            // Mirror character selection exemple create the prefab everytime we switch character in character selection
            // This state is never used in character selection (for now) only in gameplay mode.
            //m_stateMachine.HunterSelectionPose.gameObject.SetActive(false);
            // TODO: Add here if the HunterUI appears in the selection menu
        }

        public override void OnExit()
        {
            if (!m_stateMachine.IsInitialized) return;
            Debug.Log("HunterCharacterSelectionState OnExit()");
            //m_stateMachine.HunterSelectionPose.gameObject.SetActive(false);
            // TODO: Add here if the HunterUI appears in the selection menu
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