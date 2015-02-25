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

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    using ITileColumn = IImmutableDictionary<int, IEntity>;
    using ITileStore = IImmutableDictionary<int, IImmutableDictionary<int, IEntity>>;
    using IActorStore = IImmutableDictionary<long, IActorState>;

    using TileColumn = ImmutableDictionary<int, IEntity>;
    using TileStore = ImmutableDictionary<int, IImmutableDictionary<int, IEntity>>;
    using ActorStore = ImmutableDictionary<long, IActorState>;

    public class Level : ILevel
    {
        private readonly int _width;
        private readonly int _height;
        private readonly ITileStore _tiles;
        private readonly IActorStore _actorStates;

        private Level(
            int width, int height, ITileStore tiles, IActorStore actorStates)
        {
            Debug.Assert(width > 0);
            Debug.Assert(height > 0);
            Debug.Assert(tiles != null);
            Debug.Assert(actorStates != null);

            _width = width;
            _height = height;
            _tiles = tiles;
            _actorStates = actorStates;
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle(1, 1, _width, _height); }
        }

        public ITileStore Tiles
        {
            get { return _tiles; }
        }

        public IActorStore ActorStates
        {
            get { return _actorStates; }
        }

        public static ILevel Build(int width, int height)
        {
            // Populate all the X columns to save the need for on the fly creation
            var tileColumns = Enumerable
                .Range(1, width)
                .Select(x => new KeyValuePair<int, ITileColumn>(x, TileColumn.Empty));
            var tileStore = TileStore.Empty.SetItems(tileColumns);

            return new Level(width, height, tileStore, ActorStore.Empty);
        }

        public ILevel With(
            ITileStore tiles = null, IActorStore actorStates = null)
        {
            return tiles == null && actorStates == null
                ? this
                : new Level(
                    _width, 
                    _height, 
                    tiles ?? _tiles, 
                    actorStates ?? _actorStates);
        }
    }
}