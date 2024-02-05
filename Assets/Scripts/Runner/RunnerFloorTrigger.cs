using UnityEngine;

namespace Runhunt.Runner
{
    public class RunnerFloorTrigger : MonoBehaviour
    {
        private Animator m_animator;
        [field: SerializeField]
        public bool IsOnFloor { get; private set; }

        private void Awake()
        {
            m_animator = GetComponentInParent<Animator>();
            if (m_animator == null) Debug.LogError("Animator not found in parent RunnerAssets!");
        }

        private void OnTriggerStay(Collider other)
        {
            //Debug.Log("=============runner just stay collision===============");
            IsOnFloor = true;
            m_animator.SetBool("IsTouchingGround", true);
        }

        private void OnTriggerExit(Collider other)
        {
            //Debug.Log("=============runner just exit collision===============");
            IsOnFloor = false;
            m_animator.SetBool("IsTouchingGround", false);
        }
    }
}