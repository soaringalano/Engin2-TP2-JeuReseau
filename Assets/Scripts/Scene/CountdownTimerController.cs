using Mirror;
using Runhunt.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class CountdownTimerController : NetworkBehaviour
{

    [SyncVar]
    public float m_currentTimeInSecond = 180;

    [field: SerializeField] public GameObject m_countdownTimerTextMesh { get; set; }

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!isServer)
        {
            return;
        }*/
        string timetext = GameObjectHelper.GetTimeAsString(m_currentTimeInSecond);
        m_countdownTimerTextMesh.GetComponent<TextMeshProUGUI>().SetText(timetext);
    }

    void FixedUpdate()
    {
        /*if (!isServer)
        { 
            return; 
        }*/

        if(m_currentTimeInSecond >  0)
            m_currentTimeInSecond -= Time.fixedDeltaTime;
    }

    public bool TimesUp()
    {
        return m_currentTimeInSecond <= 0;
    }

}
