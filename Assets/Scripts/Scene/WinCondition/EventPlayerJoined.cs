namespace Mirror
{
    public class EventPlayerJoined : AbstractEvent
    {

        public string playerName { get; private set; }

        public EventPlayerJoined(double time, string playerName) : base(time)
        {
            this.playerName = playerName;
        }
    }
}