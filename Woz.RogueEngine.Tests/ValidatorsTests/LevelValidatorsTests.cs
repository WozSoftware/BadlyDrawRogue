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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Geometry;
using Woz.RogueEngine.Tests.TestInstances;
using Woz.RogueEngine.Validators;

namespace Woz.RogueEngine.Tests.ValidatorsTests
{
    [TestClass]
    public class LevelValidatorsTests
    {
        //[TestMethod]
        //public void IsValidMove()
        //{
        //    // Fires all the tests to ensure that IsValidMove is a combination
        //    // of all the expected validators. Should probably have one method
        //    // per test but meh, good enough for this need

        //    TileValidatorsTests.TestValidMoveTileTypes(LevelValidators.IsValidMove);
        //    TileValidatorsTests.TestValidMoveTileThings(LevelValidators.IsValidMove);
        //    TileValidatorsTests.TestValidMoveNoActor(LevelValidators.IsValidMove);
        //    TileValidatorsTests.TestInvalidMoveActorPresent(LevelValidators.IsValidMove);
        //}

        [TestMethod]
        public void IsNewActor()
        {
            Assert.IsFalse(SimpleLevel.Level
                .IsNewActor(SimpleLevel.Player.Id)
                .IsValid);

            Assert.IsTrue(SimpleLevel.Level
                .IsNewActor(0)
                .IsValid);
        }

        [TestMethod]
        public void ActorStateExists()
        {
            Assert.IsTrue(SimpleLevel.Level
                .ActorStateExists(SimpleLevel.Player.Id)
                .IsValid);

            Assert.IsFalse(SimpleLevel.Level
                .ActorStateExists(0)
                .IsValid);
        }

        [TestMethod]
        public void IsValidLocation()
        {
            Assert.IsTrue(SimpleLevel.Level
                .IsValidLocation(Vector.Create(1, 1))
                .IsValid);

            Assert.IsFalse(SimpleLevel.Level
                .IsValidLocation(Vector.Create(4, 4))
                .IsValid);
        }
    }
}