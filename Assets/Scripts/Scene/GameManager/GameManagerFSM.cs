using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mirror
{
    public class GameManagerFSM : AbstractNetworkFSM<GameState>, IObserver
    {

        public static GameManagerFSM s_instance { get; private set; }
        public bool m_gameEnded { get; private set; } = false;

        public AudioClip m_winSound;

        public ParticleSystem m_winParticles;

        public GameObject m_gameStateInfoTextMesh;

        private TextMeshProUGUI m_gameStateInfoText;

        public bool m_timesUp { get; private set; } = false;

        public bool m_playerWin { get; private set; } = false;

        // called locally when the game starts
        protected override void CreatePossibleStates()
        {
            m_possibleStates = new List<GameState>
            {
                new GameStartState(this),
                new GameEndState(this)
                //new HunterWinState(this),
                //new RunnerWinState(this)
            };
        }

        protected override void Start()
        {
            m_gameStateInfoText = m_gameStateInfoTextMesh.GetComponent<TextMeshProUGUI>();
            base.Start();
        }

        protected override void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
                base.Awake();
            }
            else
            {
                Destroy(this);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void Update()
        {
            base.Update();
        }

        // called on all clients when an event is observed
        public void OnNotify(IEvent e)
        {
            /*if(!isLocalPlayer)
            {
                return;
            }*/
            if(e.GetType() == typeof(EventRunnerArrive))
            {
                DisplayInfo(((EventRunnerArrive)e).playerName + " has arrived at the wining zone!");
            }
            else if(e.GetType() == typeof(EventRunnerWin))
            {
                m_gameEnded = true;
                DisplayInfo(((EventRunnerWin)e).playerName + " wins the game!");
                PlayEfx();
            }
            else if(e.GetType() == typeof(EventTimesUp))
            {
                m_gameEnded = true;
            }
            else if(e.GetType() == typeof(EventPlayerJoined))
            {
                DisplayInfo(((EventPlayerJoined)e).playerName + " has joined the game!");
            }
        }

        [ClientRpc]
        public void PlayEfx()
        {
            // Display win message
            if (m_winSound != null)
            {
                AudioSource.PlayClipAtPoint(m_winSound, Camera.main.transform.position);
            }
            if (m_winParticles != null)
            {
                m_winParticles.Play();
            }
        }

        [ClientRpc]
        public void DisplayInfo(string text)
        {
            Debug.Log(text);
            if(m_gameStateInfoTextMesh != null)
            {
                m_gameStateInfoText.SetText(text);
            }
        }

        [ClientRpc]
        public override void RpcChangeState(int index)
        {
            if (isServer) return;

            m_currentState.OnExit();
            m_currentState = m_possibleStates[index];
            m_currentState.OnEnter();
        }

    }

}
