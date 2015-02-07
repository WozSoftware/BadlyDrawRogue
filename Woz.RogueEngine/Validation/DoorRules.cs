using Woz.Functional.Error;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Queries;

namespace Woz.RogueEngine.Validation
{
    public static class DoorRules
    {
        public static Error<IEntity> RuleIsDoor(this IEntity entity)
        {
            return entity.EntityType == EntityType.Door
                ? entity.ToSuccees()
                : "Entity is not a door".ToError<IEntity>();
        }

        public static Error<IEntity> RuleCanOpenDoor(this IEntity entity)
        {
            return entity.HasFlagSet(EntityFlags.IsOpen)
                ? entity.ToSuccees()
                : "The door is already open".ToError<IEntity>();
        }

        public static Error<IEntity> RuleCanCloseDoor(this IEntity entity)
        {
            return !entity.HasFlagSet(EntityFlags.IsOpen)
                ? entity.ToSuccees()
                : "The door is already closed".ToError<IEntity>();
        }
    }
}