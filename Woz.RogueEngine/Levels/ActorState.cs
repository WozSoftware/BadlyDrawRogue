using System.Drawing;

namespace Woz.RogueEngine.Levels
{
    public class ActorState
    {
        private readonly long _actorId;
        private readonly Point _location;

        public ActorState(long actorId, Point location)
        {
            _actorId = actorId;
            _location = location;
        }

        public long ActorId
        {
            get { return _actorId; }
        }

        public Point Location
        {
            get { return _location; }
        }
    }
}