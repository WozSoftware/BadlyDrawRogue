#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.RogueEngine.
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
using Woz.Lenses;
using Woz.Linq.Collections;
using Woz.Monads.MaybeMonad;
using Woz.Monads.ValidationMonad;
using Woz.RogueEngine.State;
using Woz.RogueEngine.State.Lenses;
using Woz.RogueEngine.Validators;
using Woz.RogueEngine.Validators.Rules;

namespace Woz.RogueEngine.Tests.ValidatorsTests
{
    [TestClass]
    public class TileValidatorsTests
    {
        public static readonly Tile Tile = Tile.Create(
            TileTypes.Floor, "Test Tile");

        public static readonly Thing Thing = Thing.Create(
            5, ThingTypes.Item, "Test Thing",
            SlotLists.NotEquipable, EquipmentSlots.Belt.ToSome());

        [TestMethod]
        public void IsValidMoveTileType()
        {
            TestInvalidWhenTileTypeBlocksMovement(
                TileValidators.IsValidMoveTileType);
        }

        [TestMethod]
        public void IsValidMoveTileThings()
        {
            TestInvalidWhenThingTypeBlocksMovement(
                TileValidators.IsValidMoveTileThings);
        }

        [TestMethod]
        public void IsValidMoveNoActor()
        {
            TestValidWhenNoActor(TileValidators.IsValidMoveNoActor);
            TestInvalidWhenActorPresent(TileValidators.IsValidMoveNoActor);
        }

        [TestMethod]
        public void BlocksLineOfSightTileType()
        {
            TestInvalidWhenTileTypeBlocksLineOfSight(
                TileValidators.BlocksLineOfSightTileType);
        }

        [TestMethod]
        public void HasActorNoActorPresent()
        {
            Assert.IsFalse(Tile.HasActor().IsValid);
        }

        [TestMethod]
        public void HasActorActorPresent()
        {
            Assert.IsTrue(Tile
                .Set(TileLens.ActorId, 5L.ToSome())
                .HasActor()
                .IsValid);
        }

        [TestMethod]
        public void HasActorByIdNoActorPresent()
        {
            Assert.IsFalse(Tile.HasActor(1).IsValid);
        }

        [TestMethod]
        public void HasActorByIdActorPresentWrongId()
        {
            Assert.IsFalse(Tile
                .Set(TileLens.ActorId, 5L.ToSome())
                .HasActor(6)
                .IsValid);
        }

        [TestMethod]
        public void HasActorByIdActorPresentCorrectId()
        {
            Assert.IsTrue(Tile
                .Set(TileLens.ActorId, 5L.ToSome())
                .HasActor(5)
                .IsValid);
        }

        public static void TestInvalidWhenTileTypeBlocksMovement<T>(
            Func<Tile, IValidation<T>> validator)
        {
            EnumUtils
                .GetValues<TileTypes>()
                .ForEach(
                    type =>
                    {
                        var tile = Tile.Set(TileLens.TileType, type);
                        var result = validator(tile);

                        Assert.AreEqual(
                            !TileTypeRules.BlockMovement.Contains(type),
                            result.IsValid);
                    });
        }

        public static void TestInvalidWhenThingTypeBlocksMovement<T>(
            Func<Tile, IValidation<T>> validator)
        {
            EnumUtils
                .GetValues<ThingTypes>()
                .ForEach(
                    type =>
                    {
                        var thing = Thing.Set(ThingLens.ThingType, type);
                        var tile = Tile.Set(
                            TileLens.Things.Lookup(thing.Id), 
                            thing.ToSome());

                        var result = validator(tile);

                        Assert.AreEqual(
                            !ThingTypeRules.BlockMovement.Contains(type),
                            result.IsValid);
                    });
        }

        public static void TestValidWhenNoActor<T>(
            Func<Tile, IValidation<T>> validator)
        {
            Assert.IsTrue(validator(Tile).IsValid);
        }

        public static void TestInvalidWhenActorPresent<T>(
            Func<Tile, IValidation<T>> validator)
        {
            Assert.IsFalse(
                validator(Tile.Set(TileLens.ActorId, 5L.ToSome()))
                .IsValid);
        }

        public static void TestInvalidWhenTileTypeBlocksLineOfSight<T>(
            Func<Tile, IValidation<T>> validator)
        {
            EnumUtils
                .GetValues<TileTypes>()
                .ForEach(
                    type =>
                    {
                        var tile = Tile.Set(TileLens.TileType, type);
                        var result = validator(tile);

                        Assert.AreEqual(
                            !TileTypeRules.BlocksLineOfSight.Contains(type),
                            result.IsValid);
                    });
        }
    }
}
