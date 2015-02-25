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
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using Woz.Functional.Monads.MaybeMonad;
using Woz.RogueEngine.DebugHelpers;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Operations
{
    using ITileStore = IImmutableDictionary<int, IImmutableDictionary<int, IEntity>>;

    public static class TileStoreOperations
    {
        public static IEntity GetTile(
            this ITileStore tiles, Point location)
        {
            Debug.Assert(tiles != null);

            return tiles
                .Lookup(location.X)
                .SelectMany(x => x.Lookup(location.Y))
                .OrElse(EntityFactory.Void);
        }

        public static ITileStore SetTile(
            this ITileStore tiles, Point location, IEntity tile)
        {
            Debug.Assert(tiles != null);
            Debug.Assert(tile.IsValid(EntityType.Tile));

            return tiles.SetItem(
                location.X, tiles[location.X].SetItem(location.Y, tile));
        }

        public static ITileStore EditTile(
            this ITileStore tiles,
            Point location,
            Func<IEntity, IEntity> tileEditor)
        {
            Debug.Assert(tiles != null);
            Debug.Assert(tileEditor != null);

            return tiles.SetTile(
                location, tileEditor(tiles.GetTile(location)));
        }
    }
}