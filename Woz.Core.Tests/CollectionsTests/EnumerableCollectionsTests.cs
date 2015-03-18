#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Core.
//
// Woz.Core is free software: you can redistribute it 
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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woz.Core.Collections;

namespace Woz.Core.Tests.CollectionsTests
{
    [TestClass]
    public class EnumerableCollectionsTests
    {
        [TestMethod]
        public void ForEach()
        {
            var source = new[] {1, 2, 3};

            var processed = new List<int>();
            source.ForEach(processed.Add);

            CollectionAssert.AreEqual(source, processed);
        }
    }
}
