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
using System.Linq;
using Woz.Functional.Maybe;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Operations;

namespace Woz.RogueEngine.Levels
{
    public class Level : ILevel
    {
        private readonly int _width;
        private readonly int _height;
        private readonly IImmutableDictionary<int, IImmutableDictionary<int, IEntity>> _tiles;
        private readonly IImmutableDictionary<long, ActorState> _actors;

        private Level(
            int width,
            int height,
            IImmutableDictionary<int, IImmutableDictionary<int, IEntity>> tiles,
            IImmutableDictionary<long, ActorState> actors)
        {
            _width = width;
            _height = height;
            _tiles = tiles;
            _actors = actors;
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public IImmutableDictionary<int, IImmutableDictionary<int, IEntity>> Tiles
        {
            get { return _tiles; }
        }

        public IImmutableDictionary<long, ActorState> Actors
        {
            get { return _actors; }
        }

        public static ILevel Build(int width, int height)
        {

            IImmutableDictionary<int, IEntity> tilesColumn = 
                ImmutableDictionary<int, IEntity>.Empty;

            var tiles = 
                Enumerable
                    .Range(1, width)
                    .Aggregate(
                        ImmutableDictionary<int, IImmutableDictionary<int, IEntity>>.Empty,
                        (dictionary, x) => dictionary.Add(x, tilesColumn));

            return
                new Level(
                    width, height, tiles, 
                    ImmutableDictionary<long, ActorState>.Empty);
        }

        public Point GetActorLocation(long actorId)
        {
            return _actors[actorId].Location;
        }

        public IEntity GetActor(long actorId)
        {
            return GetTile(GetActorLocation(actorId)).Children[actorId];
        }

        public ILevel MoveActor(long actorId, Point newLocation)
        {
            var actorState = _actors[actorId];
            var oldLocation = actorState.Location;
            var oldlocationTile = GetTile(oldLocation);
            var actor = oldlocationTile.Children[actorId];

            var editedOldlocationTile = GetTile(oldLocation).RemoveChild(actorId);
            var editedNewLocationTile = GetTile(newLocation).AddChild(actor);
            
            return new Level(
                _width,
                _height,
                _tiles
                    .ReplaceTile(oldLocation, editedOldlocationTile)
                    .ReplaceTile(newLocation, editedNewLocationTile),
                _actors
                    .SetItem(actorId, actorState.Set(newLocation)));
        }

        public IEntity GetTile(Point location)
        {
            return _tiles[location.X]
                .Lookup(location.Y)
                .OrElse(EntityFactory.Void);
        }

        public ILevel SetTile(Point location, IEntity tile)
        {
            return new Level(
                _width, 
                _height, 
                _tiles.ReplaceTile(location, tile), 
                _actors);
        }

        public ILevel AddToTile(Point location, IEntity thing)
        {
            return SetTile(location, GetTile(location).AddChild(thing));
        }

        public ILevel RemoveFromTile(Point location, long thingId)
        {
            return SetTile(location, GetTile(location).RemoveChild(thingId));
        }
    }
}