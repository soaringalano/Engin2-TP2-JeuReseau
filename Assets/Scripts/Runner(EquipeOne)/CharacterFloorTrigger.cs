using UnityEngine;

public class CharacterFloorTrigger : MonoBehaviour
{
    [field: SerializeField]
    public bool IsOnFloor { get; private set; }

    private void OnTriggerStay(Collider other)
    {
        IsOnFloor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsOnFloor = false;
    }
}