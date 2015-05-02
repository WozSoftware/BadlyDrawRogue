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

using Woz.Core.Geometry;
using Woz.Monads.MaybeMonad;

namespace Woz.PathFinding
{
    public class LocationCandiate
    {
        private readonly Vector _location;
        private readonly int _cost;
        private readonly IMaybe<LocationCandiate> _parent;

        public LocationCandiate(
            Vector location, IMaybe<LocationCandiate> parent)
        {
            _location = location;
            _cost = 1 + parent.Select(x => x._cost).OrElse(0);
            _parent = parent;
        }

        public static LocationCandiate Create(Vector location)
        {
            return new LocationCandiate(
                location, Maybe<LocationCandiate>.None);
        }

        public static LocationCandiate Create(
            Vector location, IMaybe<LocationCandiate> parent)
        {
            return new LocationCandiate(location, parent);
        }

        public Vector Location
        {
            get { return _location; }
        }

        public int Cost
        {
            get { return _cost; }
        }

        public IMaybe<LocationCandiate> Parent
        {
            get { return _parent; }
        }
    }
}