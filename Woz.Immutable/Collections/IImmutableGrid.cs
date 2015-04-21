#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Immutable.
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

using System;
using System.Collections.Generic;
using Woz.Core.Geometry;

namespace Woz.Immutable.Collections
{
    public interface IImmutableGrid<T> : IEnumerable<Tuple<Coordinate, T>>
    {
        T this[int x, int y] { get; }
        T this[Coordinate location] { get; }
        
        int Width { get; }
        int Height { get; }
        Size Size { get; }

        bool IsValidLocation(int x, int y);
        bool IsValidLocation(Coordinate location);

        IImmutableGrid<T> Set(int x, int y, T item);
        IImmutableGrid<T> Set(Coordinate location, T item);

        ImmutableGrid<T>.Builder ToBuilder();
    }
}