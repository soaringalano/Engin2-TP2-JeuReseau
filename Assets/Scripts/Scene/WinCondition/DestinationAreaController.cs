using Mirror;
using UnityEngine;

public class DestinationAreaController : AbstractObservedObject
{

    public int playerLayer = 8;

    private double m_stayTime = 0;

    public double m_maxStayTime = 5;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    // in the settings must check the area collides only with the player
    void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }

    void OnTriggerEnter(Collider other)
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/
        Debug.Log("isLocalPlayer: " + isLocalPlayer + " isServer:" + isServer + " isClientOnly: " + isClientOnly);
        m_stayTime = 0;
        NotifyObservers(new EventRunnerArrive(Time.timeSinceLevelLoadAsDouble, other.gameObject.name));
        Debug.Log("Notified observers " + other.gameObject.name + " enter collision");
    }

    // in the settings must check the area collides only with the player
    void OnCollisionStay(Collision collision)
    {
        OnTriggerStay(collision.collider);
    }

    void OnTriggerStay(Collider other)
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/
        Debug.Log("isLocalPlayer: " + isLocalPlayer + " isServer:" + isServer + " isClientOnly: " + isClientOnly);
        m_stayTime += Time.deltaTime;
        if (m_stayTime >= m_maxStayTime)
        {
            NotifyObservers(new EventRunnerWin(Time.timeSinceLevelLoadAsDouble, other.gameObject.name));
            Debug.Log("Notified observers " + other.gameObject.name + " stay collision for " + m_stayTime + " seconds");
        }
    }

    // in the settings must check the area collides only with the player
    void OnCollisionExit(Collision collision)
    {
        OnTriggerExit(collision.collider);
    }

    void OnTriggerExit(Collider other)
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/
        Debug.Log("isLocalPlayer: " + isLocalPlayer + " isServer:" + isServer + " isClientOnly: " + isClientOnly);
        Debug.Log("Notified observers " + other.gameObject.name + " exit collision");
        m_stayTime = 0;
    }

}
