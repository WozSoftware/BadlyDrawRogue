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
    public class RouteCandidate
    {
        private readonly IMaybe<RouteCandidate> _parent;
        private readonly Vector _location;
        private readonly double _distance;
        private readonly int _cost;

        public RouteCandidate(
            IMaybe<RouteCandidate> parent, Vector location, Vector target)
        {
            _parent = parent;
            _location = location;
            _distance = location.DistanceFrom(target) * 10;
            _cost = 1 + parent.Select(x => x._cost).OrElse(0);
        }

        public static RouteCandidate Create(Vector location, Vector target)
        {
            return new RouteCandidate(
                Maybe<RouteCandidate>.None, location, target);
        }

        public static RouteCandidate Create(
            IMaybe<RouteCandidate> parent, Vector location, Vector target)
        {
            return new RouteCandidate(parent, location, target);
        }

        public IMaybe<RouteCandidate> Parent
        {
            get { return _parent; }
        }

        public Vector Location
        {
            get { return _location; }
        }

        public double Distance
        {
            get { return _distance; }
        }

        public int Cost
        {
            get { return _cost; }
        }
    }
}