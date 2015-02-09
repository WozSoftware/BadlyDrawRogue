#region License
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
using Woz.Functional.Error;

namespace Woz.Functional.Tests.ErrorTests
{
    [TestClass]
    public class ErrorOperationsTests
    {
        // ToSuccess and ToError tested in ErrorTests

        [TestMethod]
        public void CollapseWhenOuterSuccess()
        {
            var nested = 1.ToSuccess().ToSuccess();
            var collapsed = nested.Collapse();

            Assert.IsTrue(collapsed.IsValid);
            Assert.AreEqual(1, collapsed.Value);
        }

        [TestMethod]
        public void CollapseWhenOuterError()
        {
            var nested = "A".ToError<Error<int>>();
            var collapsed = nested.Collapse();

            Assert.IsFalse(collapsed.IsValid);
            Assert.AreEqual("A", collapsed.ErrorMessage);

        }
    }
}