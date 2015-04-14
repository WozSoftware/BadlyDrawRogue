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
using Woz.Monads.MaybeMonad;

namespace Woz.RogueEngine.Levels
{
    using IThingStore = IImmutableDictionary<long, Thing>;

    public sealed class Tile
    {
        public static readonly Tile Void;

        public readonly TileTypes TileType;
        public readonly string Name;
        public readonly IMaybe<Actor> Actor;
        public readonly IThingStore Things;

        static Tile()
        {
            Void = new Tile(
                TileTypes.Void,
                "Empty void",
                Maybe<Actor>.None,
                ImmutableDictionary<long, Thing>.Empty);
        }

        private Tile(
            TileTypes tileType,
            string name,
            IMaybe<Actor> actor,
            IThingStore things)
        {
            Debug.Assert(name != null);
            Debug.Assert(actor != null);
            Debug.Assert(things != null);

            TileType = tileType;
            Name = name;
            Actor = actor;
            Things = things;
        }

        public static Tile Create(
            TileTypes tileType,
            string name,
            IMaybe<Actor> actor,
            IThingStore things)
        {
            return 
                new Tile(
                    tileType,
                    name,
                    actor,
                    things);
        }

        public Tile With(
            TileTypes? tileType = null,
            string name = null,
            IMaybe<Actor> actor = null,
            IThingStore things = null)
        {
            if (TileType == TileTypes.Void)
            {
                throw new InvalidOperationException("Nothing can touch the void");
            }

            return
                !tileType.HasValue &&
                name == null &&
                actor == null &&
                things == null
                    ? this
                    : new Tile(
                        tileType ?? TileType,
                        name ?? Name,
                        actor ?? Actor,
                        things ?? Things);
        }
    }
}