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
using Woz.Immutable.Collections;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    using ITileStore = IImmutableGrid<IEntity>;
    using IActorStateStore = IImmutableDictionary<long, IActorState>;

    public class Level : ILevel
    {
        private readonly ITileStore _tiles;
        private readonly IActorStateStore _actorStates;

        private Level(ITileStore tiles, IActorStateStore actorStates)
        {
            Debug.Assert(tiles != null);
            Debug.Assert(actorStates != null);

            _tiles = tiles;
            _actorStates = actorStates;
        }

        public ITileStore Tiles
        {
            get { return _tiles; }
        }

        public IActorStateStore ActorStates
        {
            get { return _actorStates; }
        }

        public static ILevel Create(Size size)
        {
            return new Level(
                ImmutableGrid<IEntity>.Create(size),
                ImmutableDictionary<long, IActorState>.Empty);
        }

        public ILevel With(
            ITileStore tiles = null,
            IActorStateStore actors = null)
        {
            return tiles == null && actors == null
                ? this
                : new Level(tiles ?? _tiles, actors ?? _actorStates);
        }
    }
}