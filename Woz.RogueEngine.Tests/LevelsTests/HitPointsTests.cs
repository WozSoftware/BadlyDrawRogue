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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.RogueEngine.Levels;

namespace Woz.RogueEngine.Tests.LevelsTests
{
    [TestClass]
    public class HitPointsTests
    {
        public const int Maximum = 10;
        public const int Current = 8;

        public static readonly HitPoints HitPoints = 
            HitPoints.Create(Maximum, Current);

        [TestMethod]
        public void Create()
        {
            Assert.AreEqual(Maximum, HitPoints.Maximum);
            Assert.AreEqual(Current, HitPoints.Current);
        }

        [TestMethod]
        public void WithNoValues()
        {
            Assert.AreSame(HitPoints, HitPoints.With());
        }

        [TestMethod]
        public void WithMaximum()
        {
            var hitPoints = HitPoints.With(maximum: Maximum + 1);

            Assert.AreEqual(Maximum + 1, hitPoints.Maximum);
            Assert.AreEqual(Current, hitPoints.Current);
        }

        [TestMethod]
        public void WithCurrent()
        {
            var hitPoints = HitPoints.With(current: Current + 1);

            Assert.AreEqual(Maximum, hitPoints.Maximum);
            Assert.AreEqual(Current + 1, hitPoints.Current);
        }
    }
}