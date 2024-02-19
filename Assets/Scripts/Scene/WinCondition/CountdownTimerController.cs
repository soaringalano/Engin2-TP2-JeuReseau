using Mirror;
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
            Debug.Log("Client is updating time display" + timetext);
        }
        m_countdownTimerText.SetText(timetext);
    }

    void FixedUpdate()
    {
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
    }

    public void OnTimeChanged(double oldTime, double newTime)
    {
        m_clientTime = newTime;
        //Debug.Log("Server time : " + m_serverTime + " Client time : " + m_clientTime);

        string timetext = GameObjectHelper.GetTimeAsString(m_clientTime);
        //Debug.Log("Server is updating time display" + timetext);
        m_countdownTimerText.SetText(timetext);
    }

    [ClientRpc]
    public void RPCSyncTime(double time)
    {
        m_clientTime = time;
        //Debug.Log("Server time : " + m_serverTime + " Client time : " + m_clientTime);
    }

}
