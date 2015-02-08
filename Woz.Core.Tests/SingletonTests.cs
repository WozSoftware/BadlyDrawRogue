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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Woz.Core.Tests
{
    [TestClass]
    public class SingletonTests
    {
        [TestMethod]
        public void Instance()
        {
            var objectInstance = new object();
            const string stringInstance = "SomeString";

            Singleton<object>.Instance = objectInstance;
            Singleton<string>.Instance = stringInstance;

            Assert.AreSame(objectInstance, Singleton<object>.Instance);
            Assert.AreSame(stringInstance, Singleton<string>.Instance);
        }
    }
}
