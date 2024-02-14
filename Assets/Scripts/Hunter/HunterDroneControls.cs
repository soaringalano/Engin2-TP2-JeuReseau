using UnityEngine;

public class HunterDroneControls : MonoBehaviour
{
    // This class makes the drone move toward the virtual camera with a small delay
    public Transform Camera { get; set; }
    public float Speed = 1.0f;
    public float RotationSpeed = 1.0f;
    public float Delay = 0.5f;

    private float m_delayTimer = 0.0f;
    private float m_delayTime = 0.0f;
    private Vector3 m_targetPosition = Vector3.zero;
    private Quaternion m_targetRotation = Quaternion.identity;

    private void Update()
    {
        //if (Camera == null) return;

        //m_delayTimer += Time.deltaTime;
        //if (m_delayTimer > m_delayTime)
        //{
        //    m_delayTimer = 0.0f;
        //    m_delayTime = Delay;

        //    m_targetPosition = Camera.position;
        //    m_targetRotation = Camera.rotation;
        //}

        //transform.position = Vector3.Lerp(transform.position, m_targetPosition, Speed * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, RotationSpeed * Time.deltaTime);
    }
}