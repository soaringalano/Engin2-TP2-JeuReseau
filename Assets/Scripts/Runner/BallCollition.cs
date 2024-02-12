using System.Collections;
using UnityEngine;

public class BallCollition : MonoBehaviour
{
    public bool IsBallDetected { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15)
        {
            IsBallDetected = true;
            StartCoroutine(ResetBool());
        }
    }

    IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(2f);
        IsBallDetected = false;
        Debug.Log("bool reset");
    }
}
