using System.Collections;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private float m_duration;
    [SerializeField]
    private bool m_test = false;

    private void Update()
    {
        if (m_test == true)
        {
            Debug.Log("test work start");
            SetRagdollOn();
        }
    }

    public void StartRagdoll()
    {
        Debug.Log("test work start");
        SetRagdollOn();
    }

    private void SetRagdollOn()
    {
        Debug.Log("test work ragdoll on ");
        m_animator.enabled = false;
        StartCoroutine(ResetAnimator());
    }

    private void SetRagdollOff()
    {
        Debug.Log("test work ragdoll off");
        m_animator.enabled = true;
    }

    IEnumerator ResetAnimator()
    {
        yield return new WaitForSeconds(m_duration);
        Debug.Log("test work wait timer ");
        SetRagdollOff();
    }
}