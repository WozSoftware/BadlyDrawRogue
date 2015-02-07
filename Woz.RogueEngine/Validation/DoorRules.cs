using Woz.Functional.Error;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Queries;

namespace Woz.RogueEngine.Validation
{
    public static class DoorRules
    {
        public static Error<Entity> RuleIsDoor(this Entity entity)
        {
            return entity.EntityType == EntityType.Door
                ? entity.ToSuccees()
                : "Entity is not a door".ToError<Entity>();
        }

        public static Error<Entity> RuleCanOpenDoor(this Entity entity)
        {
            return entity.HasFlagSet(EntityFlags.IsOpen)
                ? entity.ToSuccees()
                : "The door is already open".ToError<Entity>();
        }

        public static Error<Entity> RuleCanCloseDoor(this Entity entity)
        {
            return !entity.HasFlagSet(EntityFlags.IsOpen)
                ? entity.ToSuccees()
                : "The door is already closed".ToError<Entity>();
        }
    }
}