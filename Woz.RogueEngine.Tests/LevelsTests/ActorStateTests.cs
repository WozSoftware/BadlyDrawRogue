#region License
// Copyright ? Woz.Software 2015
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

using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Tests.LevelsTests
{
    [TestClass]
    public class ActorStateTests
    {
        public const int Id = 1;
        public static readonly Point Location = new Point();

        public static readonly ActorState ActorState = 
            ActorState.Create(Id, Location);

        [TestMethod]
        public void Create()
        {
            Assert.AreEqual(Id, ActorState.Id);
            Assert.AreEqual(Location, ActorState.Location);
        }

        [TestMethod]
        public void WithNoValues()
        {
            Assert.AreSame(ActorState, ActorState.With());
        }

        [TestMethod]
        public void WithId()
        {
            var actorState = ActorState.With(id: 2);

            Assert.AreEqual(2, actorState.Id);
            Assert.AreEqual(Location, actorState.Location);
        }

        [TestMethod]
        public void WithLocation()
        {
            var newLocation = new Point();
            var actorState = ActorState.With(location: newLocation);

            Assert.AreEqual(Id, actorState.Id);
            Assert.AreEqual(newLocation, actorState.Location);
        }
    }
}