using System;
using UnityEngine;

public class BallHunter : MonoBehaviour
{
    public static event Action<BallHunter> BallCollitionDetected;

    private void OnTriggerEnter()
    {
        Debug.Log("collition avec la ball ");
        if (BallCollitionDetected != null)
        {
            BallCollitionDetected(this);
        }
    }
}
