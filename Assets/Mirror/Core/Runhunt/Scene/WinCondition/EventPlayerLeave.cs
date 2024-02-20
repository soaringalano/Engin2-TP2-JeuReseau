
namespace Mirror
{
    public class EventPlayerLeave : AbstractEvent
    {

        public string playerName { get; private set; }

        public EventPlayerLeave(double time, string playerName) : base(time)
        {
            this.playerName = playerName;
        }
    }
}