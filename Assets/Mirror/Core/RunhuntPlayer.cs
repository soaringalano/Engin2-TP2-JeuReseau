namespace Mirror
{
    public class RunhuntPlayer : NetworkBehaviour
    {
        [SyncVar]
        public string playerName;

        [SyncVar]
        public string m_role;
    }
}