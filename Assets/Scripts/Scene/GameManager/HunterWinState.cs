using Runhunt.FSM;

namespace Mirror
{

    // if hunter wins, then runner loses, but still need to wait for the timer to end
    public class HunterWinState : GameState
    {
        public HunterWinState(GameManagerFSM stateMachine) : base(stateMachine)
        {
        }

        public override bool CanEnter(IState currentState)
        {
            // if times up and NO runner wins, then hunter wins
            if(currentState is GameEndState)
            {
                if(m_stateMachine.m_winnedRunner == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanExit()
        {
            return false;
        }

        public override void OnEnter()
        {
            m_stateMachine.DisplayInfo("Hunter team Wins!");
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
            m_stateMachine.DisplayInfo("Hunter Wins!");
        }

    }

}