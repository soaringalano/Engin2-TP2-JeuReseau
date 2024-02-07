﻿namespace Mirror
{
    public abstract class RunnerState : IState
    {
        protected RunnerFSM m_stateMachine;

        public void OnStart(RunnerFSM stateMachine)
        {
            m_stateMachine = stateMachine;
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual bool CanEnter(IState currentState)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool CanExit()
        {
            throw new System.NotImplementedException();
        }
    }
}