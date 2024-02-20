using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    // if runner wins, then hunter loses, but still need to wait for the timer to end
    public class RunnerWinState : GameState
    {

        private List<string> winnedRunners = new List<string>();

        public RunnerWinState(GameManagerFSM stateMachine) : base(stateMachine)
        {
        }

        public override bool CanEnter(IState currentState)
        {
            // if times up and any runner wins, then THE runner wins
            // if all runners win, then don't need to wait for the timer to end, runner wins

            if (currentState is GameEndState)
            {
                return m_stateMachine.m_winnedRunner > 0;
            }
            else
            {
                return m_stateMachine.m_winnedRunner == m_stateMachine.m_runners.Count;
            }
        }

        public override bool CanExit()
        {
            return false;
        }

        public override void OnEnter()
        {
            foreach(Player p in m_stateMachine.m_runners.Values)
            {
                if(p.m_state == PlayerState.Win)
                {
                    winnedRunners.Add(p.m_name);
                }
            }

            m_stateMachine.DisplayInfo("Congratulations! Runner " + winnedRunners.ToString() + " Wins!");
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
            m_stateMachine.DisplayInfo("Congratulations! Runner " + winnedRunners.ToString() + " Wins!");
        }

    }

}