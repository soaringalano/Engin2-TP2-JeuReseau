using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    [SyncVar]
    public string m_role;

}
