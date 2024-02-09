using Mirror;
using Runhunt.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class CountdownTimerController : NetworkBehaviour
{

    [SyncVar(hook = nameof(OnTimeChanged))]
    public double m_serverTime = 180;


    private double m_clientTime;

    [SerializeField] public GameObject m_countdownTimerTextMesh;

    private TextMeshProUGUI m_countdownTimerText;

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        if(isLocalPlayer)
        {
            m_countdownTimerText = m_countdownTimerTextMesh.GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        string timetext;
        if(isServer)
        {
            timetext = GameObjectHelper.GetTimeAsString(m_serverTime);
        }
        else
        {
            timetext = GameObjectHelper.GetTimeAsString(m_clientTime);
        }
        m_countdownTimerText.SetText(timetext);
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (isServer)
            {
                if (m_serverTime > 0)
                {
                    m_serverTime -= Time.fixedDeltaTime;
                }
            }
            else if (isClient)
            {

            }
        }
    }

    public void OnTimeChanged(double oldTime, double newTime)
    {
        m_clientTime += newTime;
    }

}
