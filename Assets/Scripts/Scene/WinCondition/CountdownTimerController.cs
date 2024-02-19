﻿using Mirror;
using Runhunt.Utils;
using TMPro;
using UnityEngine;

public class CountdownTimerController : AbstractObservedObject
{

    [SyncVar(hook = nameof(OnTimeChanged))]
    public double m_serverTime = 180;


    private double m_clientTime = 0;

    public GameObject m_countdownTimerTextMesh;

    private TextMeshProUGUI m_countdownTimerText;

    void Awake()
    {
        //RegisterObserver(GameManagerFSM.s_instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
        }
        RegisterObserver(GameManagerFSM.s_instance);
        //Debug.Log("setting up countdown timer text mesh");
        m_countdownTimerText = m_countdownTimerTextMesh.GetComponent<TextMeshProUGUI>();
        /*if (m_countdownTimerText == null)
        {
            Debug.LogError("Countdown timer text mesh not found");
        }
        else
        {
            Debug.Log("Countdown timer text mesh found");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/
        //Debug.Log("is local player on update");
        string timetext;
        if (isServer)
        {
            timetext = GameObjectHelper.GetTimeAsString(m_serverTime);
            //Debug.Log("Server is updating time display" + timetext);
        }
        else
        {
            timetext = GameObjectHelper.GetTimeAsString(m_clientTime);
            //Debug.Log("Client is updating time display" + timetext);
        }
        m_countdownTimerText.SetText(timetext);
        /*if (isServer)
        {
            timetext = GameObjectHelper.GetTimeAsString(m_serverTime);
            Debug.Log("Server is updating time display" + timetext);
            m_countdownTimerText.SetText(timetext);
        }*/
    }

    void FixedUpdate()
    {
        /*if (isLocalPlayer)
        {
        }
        Debug.Log("is local player on fixedupdate");*/
        if (isServer)
        {
            if (m_serverTime > 0)
            {
                m_serverTime -= Time.fixedDeltaTime;
                RPCSyncTime(m_serverTime);
            }
            if (m_serverTime <= 0)
            {
                NotifyObservers(new EventTimesUp(Time.timeSinceLevelLoadAsDouble));
            }
        }
        else if (isClient)
        {

        }
    }

    public void OnTimeChanged(double oldTime, double newTime)
    {
        /*if (isClientOnly)
        {
        }*/
        //Debug.Log("OnTimeChanged is local player ?:" + isLocalPlayer + " is server ?:" + isServer + " is client ?:" + isClient + " old time :" + oldTime + " new time :" + newTime);
        m_clientTime = newTime;
        //Debug.Log("Server time : " + m_serverTime + " Client time : " + m_clientTime);

        string timetext = GameObjectHelper.GetTimeAsString(m_clientTime);
        //Debug.Log("Server is updating time display" + timetext);
        m_countdownTimerText.SetText(timetext);
    }

    [ClientRpc]
    public void RPCSyncTime(double time)
    {
        //Debug.Log("RPCSyncTime is local player ?:" + isLocalPlayer + " is server ?:" + isServer + " is client ?:" + isClient + " time :" + time);

        /*if (isLocalPlayer && isClientOnly)
        {
        }*/
        m_clientTime = time;
        //Debug.Log("Server time : " + m_serverTime + " Client time : " + m_clientTime);
    }

}
