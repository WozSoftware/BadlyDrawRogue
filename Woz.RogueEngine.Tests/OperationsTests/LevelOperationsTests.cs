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
using Woz.Lenses;
using Woz.RogueEngine.Operations;
using Woz.RogueEngine.State.Lenses;
using Woz.RogueEngine.Tests.TestInstances;

namespace Woz.RogueEngine.Tests.OperationsTests
{
    [TestClass]
    public class LevelOperationsTests
    {
        [TestMethod]
        public void SpawnActor()
        {
            var location = Vector.Create(0, 0);
            var monster = SimpleLevel.Monster.Set(ActorLens.Id, 3);
            var result = SimpleLevel.Level.SpawnActor(monster, location);

            var actorState = result.ActorStates[monster.Id];
            Assert.AreSame(monster, actorState.Actor);
            Assert.AreEqual(location, actorState.Location);

            Assert.AreEqual(monster.Id, result.Tiles[location].ActorId.Value);
        }

        [TestMethod]
        public void MoveActor()
        {
            var playerLocation = SimpleLevel.ActorLocation(SimpleLevel.Player);
            //var result = SimpleLevel.Level.MoveActor(
            //    SimpleLevel.Player.Id, playerLocation.)
        }
    }
}
