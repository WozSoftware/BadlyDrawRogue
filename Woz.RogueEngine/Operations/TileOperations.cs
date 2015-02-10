using System.Collections.Immutable;
using System.Drawing;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Operations
{
    public static class TileOperations
    {
        public static IImmutableDictionary<int, IImmutableDictionary<int, IEntity>>
            ReplaceTile(
            this IImmutableDictionary<int, IImmutableDictionary<int, IEntity>> tiles,
            Point location,
            IEntity tile)
        {
            return tiles
                .SetItem(
                    location.X,
                    tiles[location.X].SetItem(location.Y, tile));
        }
    }
}