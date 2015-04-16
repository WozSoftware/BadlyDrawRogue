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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Woz.Monads.MaybeMonad;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Tests.LevelsTests
{
    using IThingStore = IImmutableDictionary<long, Thing>;

    [TestClass]
    public class TileTests
    {
        public const TileTypes TileType = TileTypes.OpenDoor;
        public const string Name = "Name";
        public static readonly IMaybe<Actor> Actor = ActorTests.Actor.ToSome();
        public static readonly IThingStore Things = new Mock<IThingStore>().Object;

        public static readonly Tile Tile = 
            Tile.Create(TileType, Name, Actor, Things);

        public static void Validate(
            Tile instance,
            TileTypes? tileType = null,
            string name = null,
            IMaybe<Actor> actor = null,
            IThingStore things = null)
        {
            Assert.AreEqual(tileType ?? TileType, instance.TileType);
            Assert.AreEqual(name ?? Name, instance.Name);
            Assert.AreSame(actor ?? Actor, instance.Actor);
            Assert.AreSame(things ?? Things, instance.Things);
        }

        [TestMethod]
        public void Create()
        {
            Validate(Tile);
        }

        [TestMethod]
        public void WithNoValues()
        {
            Assert.AreSame(Tile, Tile.With());
        }

        [TestMethod]
        public void WithTileType()
        {
            Validate(
                Tile.With(tileType: TileTypes.Wall), 
                tileType: TileTypes.Wall);
        }

        [TestMethod]
        public void WithName()
        {
            Validate(Tile.With(name: "A"), name: "A");
        }

        [TestMethod]
        public void WithActor()
        {
            var actor = new Mock<IMaybe<Actor>>().Object;
            Validate(Tile.With(actor: actor), actor: actor);
        }

        [TestMethod]
        public void WithThings()
        {
            var things = new Mock<IThingStore>().Object;
            Validate(Tile.With(things: things), things: things);
        }
    }
}