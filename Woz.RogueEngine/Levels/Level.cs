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

using System.Diagnostics;
using System.Drawing;

namespace Woz.RogueEngine.Levels
{
    public class Level : ILevel
    {
        private readonly ITileManager _tiles;
        private readonly IActorManager _actors;

        private Level(ITileManager tiles, IActorManager actors)
        {
            Debug.Assert(tiles != null);
            Debug.Assert(actors != null);

            _tiles = tiles;
            _actors = actors;
        }

        public ITileManager Tiles
        {
            get { return _tiles; }
        }

        public IActorManager Actors
        {
            get { return _actors; }
        }

        public static ILevel Create(Size size)
        {
            return new Level(TileManager.Create(size), ActorManager.Create());
        }

        public ILevel With(
            ITileManager tiles = null, 
            IActorManager actors = null)
        {
            return tiles == null && actors == null
                ? this
                : new Level(tiles ?? _tiles, actors ?? _actors);
        }
    }
}