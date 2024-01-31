namespace Mirror
{
    public class RunhuntPlayer : NetworkBehaviour
    {
        [SyncVar]
        public string m_playerName;

        [SyncVar]
        public string m_role;
    }
}