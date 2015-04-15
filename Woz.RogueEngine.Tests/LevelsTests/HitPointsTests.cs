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

        private static void Validate(
            HitPoints instance, int? maximum = null, int? current = null)
        {
            Assert.AreEqual(maximum ?? Maximum, instance.Maximum);
            Assert.AreEqual(current ?? Current, instance.Current);
        }

        [TestMethod]
        public void Create()
        {
            Validate(HitPoints);
        }

        [TestMethod]
        public void WithNoValues()
        {
            Assert.AreSame(HitPoints, HitPoints.With());
        }

        [TestMethod]
        public void WithMaximum()
        {
            Validate(HitPoints.With(maximum: 20), maximum: 20);
        }

        [TestMethod]
        public void WithCurrent()
        {
            Validate(HitPoints.With(current: 1), current: 1);
        }
    }
}