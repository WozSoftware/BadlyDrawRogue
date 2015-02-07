using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Operations
{
    public static class DoorOperations
    {
        public static IEntity OpenDoor(this IEntity entity)
        {
            return entity.UpdateDoor(true);
        }

        public static IEntity CloseDoor(this IEntity entity)
        {
            return entity.UpdateDoor(false);
        }

        private static IEntity UpdateDoor(this IEntity entity, bool isOpen)
        {
            var newFlags = entity
                .Flags
                .SetItem(EntityFlags.IsOpen, isOpen)
                .SetItem(EntityFlags.BlocksMovement, !isOpen)
                .SetItem(EntityFlags.BlocksLineOfSight, !isOpen);

            return entity.Set(flags: newFlags);
        }
    }
}