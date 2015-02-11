﻿#region License
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
using System.Drawing;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    public interface ILevel
    {
        int Width { get; }
        int Height { get; }
        IImmutableDictionary<int, IImmutableDictionary<int, IEntity>> Tiles { get; }
        IImmutableDictionary<long, ActorState> Actors { get; }

        Point GetActorLocation(long actorId);
        IEntity GetActor(long actorId);
        ILevel MoveActor(long actorId, Point newLocation);

        IEntity GetTile(Point location);
        ILevel SetTile(Point location, IEntity tile);

        ILevel AddToTile(Point location, IEntity thing);
        ILevel RemoveFromTile(Point location, long thingId);
    }
}