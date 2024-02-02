using UnityEngine;

public class RunnerFloorTrigger : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    [field: SerializeField]
    public bool IsOnFloor { get; private set; }

    private void OnTriggerStay(Collider other)
    {
        IsOnFloor = true;
        m_animator.SetBool("IsTouchingGround", true);
    }

    private void OnTriggerExit(Collider other)
    {
        IsOnFloor = false;
        m_animator.SetBool("IsTouchingGround", false);
    }
}