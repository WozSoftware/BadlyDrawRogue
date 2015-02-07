using Woz.Functional.Error;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Validation
{
    public static class DoorValidators
    {
        public static Error<IEntity> CanOpenDoor(this IEntity entity)
        {
            return entity
                .ToSuccees()
                .SelectMany(x => x.RuleIsDoor())
                .SelectMany(x => x.RuleCanOpenDoor());
        }

        public static Error<IEntity> CanCloseDoor(this IEntity entity)
        {
            return entity
                .ToSuccees()
                .SelectMany(x => x.RuleIsDoor())
                .SelectMany(x => x.RuleCanCloseDoor());
        }
    }
}