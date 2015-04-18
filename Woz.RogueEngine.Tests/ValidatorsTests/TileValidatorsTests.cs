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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core;
using Woz.Core.Collections;
using Woz.Lenses;
using Woz.Monads.MaybeMonad;
using Woz.Monads.ValidationMonad;
using Woz.RogueEngine.Levels;
using Woz.RogueEngine.Tests.LevelsTests;
using Woz.RogueEngine.Validators;

namespace Woz.RogueEngine.Tests.ValidatorsTests
{
    [TestClass]
    public class TileValidatorsTests
    {
        private readonly Tile _tile = Tile.Create(
            TileTypes.Floor, "Test Tile");

        private readonly Thing _thing = Thing.Create(
            5, ThingTypes.Item, "Test Thing", EquipmentSlots.None, false);

        [TestMethod]
        public void IsValidMove()
        {
            // Fires all the tests to ensure that IsValidMove is a combination
            // of all the expected validators. Should probably have one method
            // per test but meh, good enough for this need

            TestValidMoveTileTypes(TileValidators.IsValidMove);
            TestValidMoveTileThings(TileValidators.IsValidMove);
            TestValidMoveNoActor(TileValidators.IsValidMove);
            TestInvalidMoveActorPresent(TileValidators.IsValidMove);
        }

        [TestMethod]
        public void IsValidMoveTileType()
        {
            TestValidMoveTileTypes(TileValidators.IsValidMoveTileType);
        }

        [TestMethod]
        public void IsValidMoveTileThings()
        {
            TestValidMoveTileThings(TileValidators.IsValidMove);
        }

        [TestMethod]
        public void IsValidMoveNoActor()
        {
            TestValidMoveNoActor(TileValidators.IsValidMoveNoActor);
            TestInvalidMoveActorPresent(TileValidators.IsValidMoveNoActor);
        }

        [TestMethod]
        public void HasActorNoActorPresent()
        {
            Assert.IsFalse(_tile.HasActor().IsValid);
        }

        [TestMethod]
        public void HasActorActorPresent()
        {
            Assert.IsTrue(_tile
                .Set(TileLens.Actor, ActorTests.Actor.ToSome())
                .HasActor()
                .IsValid);
        }

        [TestMethod]
        public void HasActorByIdNoActorPresent()
        {
            Assert.IsFalse(_tile.HasActor(1).IsValid);
        }

        [TestMethod]
        public void HasActorByIdActorPresentWrongId()
        {
            Assert.IsFalse(_tile
                .Set(TileLens.Actor, ActorTests.Actor.ToSome())
                .HasActor(ActorTests.Actor.Id + 1)
                .IsValid);
        }

        [TestMethod]
        public void HasActorByIdActorPresentCorrectId()
        {
            Assert.IsTrue(_tile
                .Set(TileLens.Actor, ActorTests.Actor.ToSome())
                .HasActor(ActorTests.Actor.Id)
                .IsValid);
        }

        public void TestValidMoveTileTypes<T>(
            Func<Tile, IValidation<T>> validator)
        {
            EnumUtils
                .GetValues<TileTypes>()
                .ForEach(
                    type =>
                    {
                        var tile = _tile.Set(TileLens.TileType, type);
                        var result = validator(tile);

                        Assert.AreEqual(
                            !TileTypeGroups.BlockMovement.Contains(type),
                            result.IsValid);
                    });
        }

        public void TestValidMoveTileThings<T>(
            Func<Tile, IValidation<T>> validator)
        {
            EnumUtils
                .GetValues<ThingTypes>()
                .ForEach(
                    type =>
                    {
                        var thing = _thing.Set(ThingLens.ThingType, type);
                        var tile = _tile.Set(TileLens.Things.Lookup(thing.Id), thing.ToSome());

                        var result = validator(tile);

                        Assert.AreEqual(
                            !ThingTypeGroups.BlockMovement.Contains(type),
                            result.IsValid);
                    });
        }

        public void TestValidMoveNoActor<T>(
            Func<Tile, IValidation<T>> validator)
        {
            Assert.IsTrue(validator(_tile).IsValid);
        }

        public void TestInvalidMoveActorPresent<T>(
            Func<Tile, IValidation<T>> validator)
        {
            Assert.IsFalse(
                validator(_tile.Set(TileLens.Actor, ActorTests.Actor.ToSome()))
                .IsValid);
        }
    }
}
