using UnityEngine;

public class CharacterFloorTrigger : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    [field: SerializeField]
    public bool IsOnFloor { get; private set; }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("==========Now character is on floor");
        IsOnFloor = true;
        m_animator.SetBool("IsTouchingGround", true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("==========Now character is in air");
        IsOnFloor = false;
        m_animator.SetBool("IsTouchingGround", false);
    }
}