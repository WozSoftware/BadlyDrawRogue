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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Woz.Functional.Monads.MaybeMonad;
using Woz.RogueEngine.DebugHelpers;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    public class TileManager : ITileManager
    {
        private readonly Size _size;
        private readonly IImmutableDictionary<int, IImmutableDictionary<int, IEntity>> _tiles;

        public TileManager(
            Size size,
            IImmutableDictionary<int, IImmutableDictionary<int, IEntity>> tiles)
        {
            Debug.Assert(size.Width > 0);
            Debug.Assert(size.Height > 0);

            _size = size;
            _tiles = tiles;
        }

        public Size Size
        {
            get { return _size; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle(1, 1, _size.Width, _size.Height); }
        }

        public IEntity this[Point location]
        {
            get
            {
                return _tiles
                    .Lookup(location.X)
                    .SelectMany(x => x.Lookup(location.Y))
                    .OrElse(EntityFactory.Void);
            }
        }

        public static ITileManager Build(Size size)
        {
            // Populate all the X columns to save the need for on the fly creation
            var tileColumns = Enumerable
                .Range(1, size.Width)
                .Select(x => 
                    new KeyValuePair<int, IImmutableDictionary<int, IEntity>>(
                        x, ImmutableDictionary<int, IEntity>.Empty));
            
            var tiles = 
                ImmutableDictionary<int, IImmutableDictionary<int, IEntity>>
                    .Empty
                    .SetItems(tileColumns);

            return new TileManager(size, tiles);
        }

        public ITileManager SetTile(Point location, IEntity tile)
        {
            Debug.Assert(tile.IsValid(EntityType.Tile));

            return new TileManager(
                _size, 
                _tiles.SetItem(
                    location.X, 
                    _tiles[location.X].SetItem(location.Y, tile)));
        }

        public ITileManager EditTile(Point location, Func<IEntity, IEntity> tileEditor)
        {
            return SetTile(location, tileEditor(this[location]));
        }

        public IEnumerator<Tuple<Point, IEntity>> GetEnumerator()
        {
            return _tiles
                .SelectMany(
                    x => x.Value, 
                    (x, y) => Tuple.Create(new Point(x.Key, y.Key), y.Value))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}