#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.RoqueEngine.
//
// Woz.RoqueEngine is free software: you can redistribute it 
// and/or modify it under the terms of the GNU General Public 
// License as published by the Free Software Foundation, either 
// version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion
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