using System;
using UnityEngine;

public class BallHunter : MonoBehaviour
{
    public static event Action<BallHunter> BallCollition;

    private void OnTriggerEnter()
    {
        if (BallCollition != null)
        {
            BallCollition(this);
        }
    }
}
