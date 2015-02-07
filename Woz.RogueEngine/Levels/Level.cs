//using System;
//using System.Collections.Immutable;
//using System.Configuration;
//using System.Drawing;
//using Functional.Maybe;
//using Woz.Functional.Maybe;
//using Woz.RogueEngine.Entities;
//using Woz.RogueEngine.Queries;

//namespace Woz.RogueEngine.Levels
//{
//    public class Level
//    {
//        // TILES WRONG - More collections
//        private readonly IImmutableDictionary<Point, Entity> _tiles;
//        private readonly Maybe<ActorState> _player;
//        private readonly IImmutableDictionary<long, ActorState> _npcs;

//        public Level(IImmutableDictionary<Point, Entity> tiles)
//            : this(
//                tiles,
//                Maybe<ActorState>.Nothing,
//                ImmutableDictionary<long, ActorState>.Empty)
//        {
//        }

//        private Level(
//            IImmutableDictionary<Point, Entity> tiles,
//            Maybe<ActorState> player,
//            IImmutableDictionary<long, ActorState> npcs)
//        {
//            _tiles = tiles;
//            _player = player;
//            _npcs = npcs;
//        }

//        public Maybe<ActorState> Player
//        {
//            get { return _player; }
//        }

//        public Maybe<ActorState> GetActorAt(Point location)
//        {
//            var actor = GetTile(location)
//                .Children
//                .WhereFlagSet(EntityFlags.IsActor)
//                .FirstMaybe();

//            if (actor.IsNothing())
//            {
//                return Maybe<ActorState>.Nothing;
//            }

//            return
//                actor.Value.HasFlagSet(EntityFlags.IsPlayer)
//                    ? _player
//                    : _npcs[actor.Value.Id].ToMaybe();
//        }

//        public Level SpawnNpc(Point location, Entity npc)
//        {
//            var tile = GetTile(location);

//        }

//        public Entity GetTile(Point location)
//        {
//            return _tiles
//                .Lookup(location)
//                .OrElse(() =>
//                    new InvalidOperationException(
//                        string.Format("No tile at {0}", location)));
//        }
//    }
//}