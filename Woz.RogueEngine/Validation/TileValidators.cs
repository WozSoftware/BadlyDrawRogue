using Woz.Functional.Error;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Validation
{
    public static class TileValidators
    {
        public static Error<IEntity> CanOpenDoor(this IEntity entity)
        {
            return entity
                .ToSuccees()
                .SelectMany(x => x.RuleIsTile())
                .SelectMany(x => x.RuleIsTileType(TileTypes.Door))
                .SelectMany(x => x.RuleCanOpenDoor());
        }

        public static Error<IEntity> CanCloseDoor(this IEntity entity)
        {
            return entity
                .ToSuccees()
                .SelectMany(x => x.RuleIsTile())
                .SelectMany(x => x.RuleIsTileType(TileTypes.Door))
                .SelectMany(x => x.RuleCanCloseDoor());
        }
    }
}