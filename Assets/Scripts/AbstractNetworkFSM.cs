using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class AbstractNetworkFSM<T> : NetworkBehaviour where T : IState
    {

        protected T m_currentState;
        
        protected List<T> m_possibleStates;
        static public Transform Scene { get; private set; }
        

        protected virtual void Awake()
        {
            CreatePossibleStates();
        }

        protected virtual void Start()
        {
            Scene = GetScene(gameObject).transform;
            if (Scene == null) Debug.LogError("Scene not found!");

            foreach (IState state in m_possibleStates)
            {
                state.OnStart();
            }
            m_currentState = m_possibleStates[0];
            m_currentState.OnEnter();
        }

        protected virtual void Update()
        {
            //Debug.Log("Current state:" + m_currentState.GetType());
            m_currentState.OnUpdate();
            TryStateTransition();
        }

        protected virtual void FixedUpdate()
        {
            //Debug.Log("Current state:" + m_currentState.GetType());
            m_currentState.OnFixedUpdate();
        }

        protected virtual void CreatePossibleStates()
        {

        }

        protected void TryStateTransition()
        {

            if (!m_currentState.CanExit())
            {
                return;
            }

            //Je PEUX quitter le state actuel
            //foreach (var state in m_possibleStates)
            for(int i=0;i<m_possibleStates.Count;i++)
            {
                var state = m_possibleStates[i];
                if (m_currentState.Equals(state))
                {
                    continue;
                }

                if (state.CanEnter(m_currentState))
                {
                    //Quitter le state actuel
                    m_currentState.OnExit();
                    m_currentState = state;
                    //Rentrer dans le state state
                    m_currentState.OnEnter();

                    //sync state change to clients
                    RpcChangeState(i);
                    
                    
                    return;
                }
            }
        }

        public virtual void RpcChangeState(int index)
        {
        }

        static public GameObject GetScene(GameObject characterGO)
        {
            // Source : https://discussions.unity.com/t/find-gameobjects-in-specific-scene-only/163901
            GameObject[] gameObjects = characterGO.scene.GetRootGameObjects();
            GameObject sceneGO = null;

            foreach (GameObject _gameObject in gameObjects)
            {
                if (_gameObject.name != "Scene") continue;

                sceneGO = _gameObject;
                break;
            }

            return sceneGO;
        }
    }
}