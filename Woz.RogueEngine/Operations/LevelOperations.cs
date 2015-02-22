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
using System.Drawing;
using Woz.Functional.Monads.MaybeMonad;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Operations
{
    public static class LevelOperations
    {
        public static IEntity GetTile(
            this ILevel level, Point location)
        {
            return level
                .Tiles
                .Lookup(location.X)
                .SelectMany(x => x.Lookup(location.Y))
                .OrElse(EntityFactory.Void);
        }

        public static ILevel SetTile(
            this ILevel level, Point location, IEntity tile)
        {
            return level.With(tiles: level.Tiles.ReplaceTile(location, tile));
        }

        private static IImmutableDictionary<int, IImmutableDictionary<int, IEntity>>
            ReplaceTile(
            this IImmutableDictionary<int, IImmutableDictionary<int, IEntity>> tiles,
            Point location,
            IEntity tile)
        {
            return tiles
                .SetItem(
                    location.X,
                    tiles[location.X].SetItem(location.Y, tile));
        }

        //public ILevel MoveActor(long actorId, Point newLocation)
        //{
        //    var actorState = _actors[actorId];
        //    var oldLocation = actorState.Location;
        //    var oldlocationTile = GetTile(oldLocation);
        //    var actor = oldlocationTile.Children[actorId];

        //    var editedOldlocationTile = GetTile(oldLocation).RemoveChild(actorId);
        //    var editedNewLocationTile = GetTile(newLocation).AddChild(actor);

        //    return new Level(
        //        _width,
        //        _height,
        //        _tiles
        //            .ReplaceTile(oldLocation, editedOldlocationTile)
        //            .ReplaceTile(newLocation, editedNewLocationTile),
        //        _actors
        //            .SetItem(actorId, actorState.Set(newLocation)));
        //}

        //public ILevel AddToTile(Point location, IEntity thing)
        //{
        //    return SetTile(location, GetTile(location).AddChild(thing));
        //}

        //public ILevel RemoveFromTile(Point location, long thingId)
        //{
        //    return SetTile(location, GetTile(location).RemoveChild(thingId));
        //}
         
    }
}