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
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    using TileStore = IImmutableDictionary<int, IImmutableDictionary<int, IEntity>>;
    using ActorStore = IImmutableDictionary<long, IActorState>;

    public class Level : ILevel
    {
        private readonly int _width;
        private readonly int _height;
        private readonly TileStore _tiles;
        private readonly ActorStore _actorStates;

        private Level(
            int width, int height, TileStore tiles, ActorStore actorStates)
        {
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

        public TileStore Tiles
        {
            get { return _tiles; }
        }

        public ActorStore ActorStates
        {
            get { return _actorStates; }
        }

        public static ILevel Build(int width, int height)
        {
            return
                new Level(
                    width, height,
                    ImmutableDictionary<int, IImmutableDictionary<int, IEntity>>.Empty, 
                    ImmutableDictionary<long, IActorState>.Empty);
        }

        public ILevel With(
            TileStore tiles = null, ActorStore actorStates = null)
        {
            return tiles == null && actorStates == null
                ? this
                : new Level(_width, _height, tiles, actorStates);
        }
    }
}