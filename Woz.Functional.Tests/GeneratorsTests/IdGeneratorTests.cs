﻿#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Functional.
//
// Woz.Functional is free software: you can redistribute it 
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
using Woz.Functional.Generators;

namespace Woz.Functional.Tests.GeneratorsTests
{
    [TestClass]
    public class IdGeneratorTests
    {
        [TestMethod]
        public void Build()
        {
            var idGenerator = IdGenerator.Build();

            Assert.AreEqual(1, idGenerator());
            Assert.AreEqual(2, idGenerator());
            Assert.AreEqual(3, idGenerator());
        }
    }
}
