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
namespace Woz.Core
{
    /// <summary>
    /// Object used to provide the singleton pattern to a non static class. 
    /// Relies on the fact that generic static classes build out to a new 
    /// instance per type.
    /// </summary>
    /// <typeparam name="T">Type of the singleton</typeparam>
    public static class Singleton<T>
        where T : class
    {
        public static T Instance { get; set; }
    }
}