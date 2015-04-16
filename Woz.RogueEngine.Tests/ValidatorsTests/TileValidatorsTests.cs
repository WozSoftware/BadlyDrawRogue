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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core;
using Woz.Core.Collections;
using Woz.RogueEngine.Levels;
using Woz.RogueEngine.Tests.LevelsTests;
using Woz.RogueEngine.Validators;

namespace Woz.RogueEngine.Tests.ValidatorsTests
{
    [TestClass]
    public class TileValidatorsTests
    {
        [TestMethod]
        public void TileTypeAllowsMove()
        {
            EnumUtils
                .GetValues<TileTypes>()
                .ForEach(
                    type =>
                    {
                        var result = TileTests
                            .Tile
                            .With(tileType: type)
                            .TileTypeAllowsMove();

                        Assert.AreEqual(
                            !TypeGroups.BlockMovement.Contains(type),
                            result.IsValid);
                    });
        }
    }
}
