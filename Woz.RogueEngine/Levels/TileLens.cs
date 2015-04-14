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
using Woz.Lenses;
using Woz.Monads.MaybeMonad;

namespace Woz.RogueEngine.Levels
{
    using IThingStore = IImmutableDictionary<long, Thing>;

    public static class TileLens
    {
        public static readonly Lens<Tile, TileTypes> TileType;
        public static readonly Lens<Tile, string> Name;
        public static readonly Lens<Tile, IMaybe<Actor>> Actor;
        public static readonly Lens<Tile, IThingStore> Things;

        static TileLens()
        {
            TileType = Lens.Create<Tile, TileTypes>(
                tile => tile.TileType,
                tileType => tile => tile.With(tileType: tileType));

            Name = Lens.Create<Tile, string>(
                tile => tile.Name,
                name => tile => tile.With(name: name));

            Actor = Lens.Create<Tile, IMaybe<Actor>>(
                tile => tile.Actor,
                actor => tile => tile.With(actor: actor));

            Things = Lens.Create<Tile, IThingStore>(
                tile => tile.Things,
                things => tile => tile.With(things: things));
        }
    }
}