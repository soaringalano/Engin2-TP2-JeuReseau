using System.Collections;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public bool IsBallCollisionDetected { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15)
        {
            IsBallCollisionDetected = true;
            StartCoroutine(ResetBool());
        }
    }

    IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(2f);
        IsBallCollisionDetected = false;
        Debug.Log("bool reset");
    }
}
