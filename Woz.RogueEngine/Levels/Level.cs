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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Woz.Core.Collections;
using Woz.Core.Coordinates;
using Woz.Immutable.Collections;

namespace Woz.RogueEngine.Levels
{
    using ITileStore = IImmutableGrid<Tile>;
    using IActorStateStore = IImmutableDictionary<long, ActorState>;

    public sealed class Level 
    {
        public readonly ITileStore Tiles;
        public readonly IActorStateStore ActorStates;

        private Level(ITileStore tiles, IActorStateStore actorStates)
        {
            Debug.Assert(tiles != null);
            Debug.Assert(actorStates != null);

            Tiles = tiles;
            ActorStates = actorStates;
        }

        public static Level Create(Size size)
        {
            var walker =
                from x in Enumerable.Range(0, size.Width)
                from y in Enumerable.Range(0, size.Height)
                select new Coordinate(x, y);

            var gridBuilder = ImmutableGrid<Tile>.CreateBuilder(size);
            walker.ForEach(location => gridBuilder.Set(location, Tile.Void));

            return new Level(
                gridBuilder.Build(),
                ImmutableDictionary<long, ActorState>.Empty);
        }

        public static Level Create(ITileStore tiles, IActorStateStore actorStates)
        {
            return new Level(tiles, actorStates);
        }

        public Level With(
            ITileStore tiles = null,
            IActorStateStore actorStates = null)
        {
            return tiles != null || actorStates != null
                ? new Level(tiles ?? Tiles, actorStates ?? ActorStates)
                : this;
        }
    }
}