using Woz.Functional.Error;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Validation
{
    public static class DoorValidators
    {
        public static Error<Entity> CanOpenDoor(this Entity entity)
        {
            return entity
                .ToSuccees()
                .SelectMany(x => x.RuleIsDoor())
                .SelectMany(x => x.RuleCanOpenDoor());
        }

        public static Error<Entity> CanCloseDoor(this Entity entity)
        {
            return entity
                .ToSuccees()
                .SelectMany(x => x.RuleIsDoor())
                .SelectMany(x => x.RuleCanCloseDoor());
        }
    }
}