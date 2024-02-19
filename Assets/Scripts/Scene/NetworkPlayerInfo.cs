using Mirror;
using UnityEngine;

namespace Mirror
{
    public class NetworkPlayerInfo : NetworkBehaviour
    {

        [field: SerializeField]
        public string m_name { get; set; }

        [field: SerializeField]
        public PlayerTeam m_team { get; set; }

        [field: SerializeField]
        public PlayerState m_state { get; set; }

    }

}

public enum PlayerTeam
{
    Runner, Hunter
}

public enum PlayerState
{
    Win, Lose, Playing
}
