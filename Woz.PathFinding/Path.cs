#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.PathFinding.
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

using System.Collections.Immutable;
using Woz.Core.Geometry;

namespace Woz.PathFinding
{
    public class Path
    {
        private readonly Vector _end;
        private readonly ImmutableStack<Vector> _route;

        private Path(Vector end, ImmutableStack<Vector> route)
        {
            _end = end;
            _route = route;
        }

        public static Path Create(
            Vector end, ImmutableStack<Vector> route)
        {
            return new Path(end, route);
        }

        public Vector End
        {
            get { return _end; }
        }

        public ImmutableStack<Vector> Route
        {
            get { return _route; }
        }

        public Vector NextLocation
        {
            get { return _route.Peek(); }
        }

        public Path ConsumeNextLocation()
        {
            return new Path(_end, _route.Pop());
        }
    }
}