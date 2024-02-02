using UnityEngine;

public class RunnerFloorTrigger : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    [field: SerializeField]
    public bool IsOnFloor { get; private set; }

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