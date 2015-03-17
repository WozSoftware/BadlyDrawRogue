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
using Woz.Immutable.Collections;
using Woz.Lenses;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    using ITileStore = IImmutableGrid<Entity>;
    using IActorStateStore = IImmutableDictionary<long, IActorState>;

    public static class LevelLens
    {
        public static readonly Lens<Level, ITileStore> Tiles;
        public static readonly Lens<Level, IActorStateStore> ActorStates;

        static LevelLens()
        {
            Tiles = Lens.Create<Level, ITileStore>(
                level => level.Tiles,
                tile => level => level.With(tiles: tile));

            ActorStates = Lens.Create<Level, IActorStateStore>(
                level => level.ActorStates,
                actorStates => level => level.With(actorStates: actorStates));
        }
    }
}