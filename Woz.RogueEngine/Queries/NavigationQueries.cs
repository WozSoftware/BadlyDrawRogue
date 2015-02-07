using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Queries
{
    public static class NavigationQueries
    {
        public static bool BlocksLineOfSight(this IEntity entity)
        {
            return entity.TreeHasFlagSet(EntityFlags.BlocksLineOfSight);
        }

        public static bool BlocksMovement(this IEntity entity)
        {
            return entity.TreeHasFlagSet(EntityFlags.BlocksMovement);
        }
    }
}