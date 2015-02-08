using System;
using Woz.Functional.Error;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Queries;

namespace Woz.RogueEngine.Validation
{
    public static class TileRules
    {
        public static Error<IEntity> RuleIsTile(this IEntity entity)
        {
            return entity.EntityType == EntityType.Tile
                ? entity.ToSuccees()
                : "Entity is not a tile".ToError<IEntity>();
        }

        public static Error<IEntity> RuleIsTileType(this IEntity entity, TileTypes tileType)
        {
            return entity.RuleAttributeHasEnumValue(
                EntityAttributes.TileType,
                tileType,
                () => string.Format("Entity is not a {0}", tileType));
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