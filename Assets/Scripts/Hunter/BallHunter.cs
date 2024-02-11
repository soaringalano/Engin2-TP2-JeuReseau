using System;
using UnityEngine;

public class BallHunter : MonoBehaviour
{
    public static event Action<BallHunter> BallCollisionDetected;

    private void OnTriggerEnter()
    {
        Debug.Log("collition avec la ball ");
        if (BallCollisionDetected != null)
        {
            BallCollisionDetected(this);
        }
    }
}
