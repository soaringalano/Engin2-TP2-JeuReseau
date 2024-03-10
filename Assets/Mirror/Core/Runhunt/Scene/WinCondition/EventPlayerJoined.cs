using System;

namespace Mirror
{
    public class EventPlayerJoined : AbstractEvent
    {

        public string playerName { get; private set; }

        public PlayerTeam playerTeam { get; private set; }

        public EventPlayerJoined(double time, string playerName, PlayerTeam team) : base(time)
        {
            this.playerName = playerName;
            this.playerTeam = team;
        }

        public EventPlayerJoined(double time, string playerName, string team) : base(time)
        {
            this.playerName = playerName;

            PlayerTeam output;
            Enum.TryParse<PlayerTeam>(team, true, out output);
            this.playerTeam = output;
        }
    }
}