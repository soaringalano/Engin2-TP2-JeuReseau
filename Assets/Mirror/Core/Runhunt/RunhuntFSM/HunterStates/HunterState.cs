﻿namespace Mirror
{
    public abstract class HunterState : IState
    {
        protected HunterFSM m_stateMachine;

        public void OnStart(HunterFSM stateMachine)
        {
            m_stateMachine = stateMachine;
        }

        public virtual bool CanEnter(IState currentState)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool CanExit()
        {
            throw new System.NotImplementedException();
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

        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate()
        {
        }
    }
}