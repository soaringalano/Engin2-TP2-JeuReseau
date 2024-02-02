using UnityEngine;

public class RunnerFloorTrigger : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    [field: SerializeField]
    public bool IsOnFloor { get; private set; }

    private void OnTriggerStay(Collider other)
    {
<<<<<<< HEAD:Assets/Scripts/Runner/CharacterFloorTrigger.cs
        //Debug.Log("==========Now character is on floor");
=======
        //Debug.Log("=============runner just stay collision===============");
>>>>>>> main:Assets/Scripts/Runner/RunnerFloorTrigger.cs
        IsOnFloor = true;
        m_animator.SetBool("IsTouchingGround", true);
    }

    private void OnTriggerExit(Collider other)
    {
<<<<<<< HEAD:Assets/Scripts/Runner/CharacterFloorTrigger.cs
        //Debug.Log("==========Now character is in air");
=======
        //Debug.Log("=============runner just exit collision===============");
>>>>>>> main:Assets/Scripts/Runner/RunnerFloorTrigger.cs
        IsOnFloor = false;
        m_animator.SetBool("IsTouchingGround", false);
    }
}