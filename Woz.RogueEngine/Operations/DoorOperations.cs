using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Operations
{
    public static class DoorOperations
    {
        public static Entity OpenDoor(this Entity entity)
        {
            return entity.UpdateDoor(true);
        }

        public static Entity CloseDoor(this Entity entity)
        {
            return entity.UpdateDoor(false);
        }

        private static Entity UpdateDoor(this Entity entity, bool isOpen)
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