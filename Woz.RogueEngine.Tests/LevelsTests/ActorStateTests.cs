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
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Tests.LevelsTests
{
    [TestClass]
    public class ActorStateTests
    {
        public const int Id = 1;
        public static readonly Vector Location = new Vector();

        public static readonly ActorState ActorState = 
            ActorState.Create(Id, Location);

        private static void Validate(
            ActorState instance, long? id = null, Vector? location = null)
        {
            Assert.AreEqual(id ?? Id, instance.Id);
            Assert.AreEqual(location ?? Location, instance.Location);
        }

        [TestMethod]
        public void Create()
        {
            Validate(ActorState);
        }

        [TestMethod]
        public void WithNoValues()
        {
            Assert.AreSame(ActorState, ActorState.With());
        }

        [TestMethod]
        public void WithId()
        {
            Validate(ActorState.With(id: 2), id: 2);
        }

        [TestMethod]
        public void WithLocation()
        {
            var newLocation = new Vector();
            Validate(
                ActorState.With(location: newLocation), 
                location: newLocation);
        }
    }
}