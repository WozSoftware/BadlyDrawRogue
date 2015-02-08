using System.Collections.Immutable;
using System.Linq;
using Functional.Maybe;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    public class Level
    {
        private static readonly IEntity VoidTile = Entity.Build(0, EntityType.Tile, "Void");

        private readonly IImmutableList<IImmutableList<IEntity>> _tiles;
        private readonly Maybe<ActorState> _player;
        private readonly IImmutableDictionary<long, ActorState> _npcs;

        private Level(
            IImmutableList<IImmutableList<IEntity>> tiles,
            Maybe<ActorState> player,
            IImmutableDictionary<long, ActorState> npcs)
        {
            _tiles = tiles;
            _player = player;
            _npcs = npcs;
        }

        public IImmutableList<IImmutableList<IEntity>> Tiles
        {
            get { return _tiles; }
        }

        public Maybe<ActorState> Player
        {
            get { return _player; }
        }

        public IImmutableDictionary<long, ActorState> Npcs
        {
            get { return _npcs; }
        }

        public static Level Build(int width, int height)
        {
            IImmutableList<IEntity> tilesColumn = 
                Enumerable.Repeat(VoidTile, height).ToImmutableList();
            
            IImmutableList<IImmutableList<IEntity>> tiles = 
                Enumerable.Repeat(tilesColumn, width).ToImmutableList();

            return
                new Level(
                    tiles, 
                    Maybe<ActorState>.Nothing, 
                    ImmutableDictionary<long, ActorState>.Empty);
        }

        //public Maybe<ActorState> GetActorAt(Point location)
        //{
        //    var actor = GetTile(location)
        //        .Children
        //        .WhereFlagSet(EntityFlags.IsActor)
        //        .FirstMaybe();

        //    if (actor.IsNothing())
        //    {
        //        return Maybe<ActorState>.Nothing;
        //    }

        //    return
        //        actor.Value.HasFlagSet(EntityFlags.IsPlayer)
        //            ? _player
        //            : _npcs[actor.Value.Id].ToMaybe();
        //}

        //public Level SpawnNpc(Point location, Entity npc)
        //{
        //    var tile = GetTile(location);

        //}

        //public Entity GetTile(Point location)
        //{
        //    return _tiles
        //        .Lookup(location)
        //        .OrElse(() =>
        //            new InvalidOperationException(
        //                string.Format("No tile at {0}", location)));
        //}
    }
}