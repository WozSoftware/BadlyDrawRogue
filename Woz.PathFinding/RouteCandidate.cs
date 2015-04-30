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
        public readonly IMaybe<RouteCandidate> Parent;
        public readonly Vector Location;
        public readonly double Distance;
        public readonly int Cost;

        public RouteCandidate(
            IMaybe<RouteCandidate> parent, Vector location, Vector target)
        {
            Parent = parent;
            Location = location;
            Distance = location.DistanceFrom(target) * 10;
            Cost = 1 + Parent.Select(x => x.Cost).OrElse(0);
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
    }
}