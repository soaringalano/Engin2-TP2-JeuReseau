using System.Collections;
using UnityEngine;

namespace Runhunt.Runner
{
<<<<<<< HEAD
    public class RunnerFloorTrigger : MonoBehaviour
=======
    [SerializeField]
    private Animator m_animator;
    [field: SerializeField]
    public bool IsOnFloor { get; private set; }
    [field: SerializeField]
    public bool ISDetectMine { get; private set; }

    private void OnTriggerStay(Collider other)
>>>>>>> ragdollsavetest
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

    private void OnTriggerEnter(Collider other)
    {
        ISDetectMine = true;
        StartCoroutine(ResetBool());
    }
    IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(2f);
        ISDetectMine = false;
        Debug.Log("bool reset");
    }
}