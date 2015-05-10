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
        public const int DistanceMultiplier = 10;

        private readonly Vector _location;
        private readonly int _currentCost;
        private readonly int _remainingEstimatedCost;
        private readonly IMaybe<LocationCandiate> _parent;

        private LocationCandiate(
            Vector location, 
            int currentCost, 
            int remnainingEstimatedCost, 
            IMaybe<LocationCandiate> parent)
        {
            _location = location;
            _currentCost = currentCost;
            _remainingEstimatedCost = remnainingEstimatedCost;
            _parent = parent;
        }

        public static LocationCandiate Create(Vector target, Vector location)
        {
            return Create(target, location, Maybe<LocationCandiate>.None);
        }

        public static LocationCandiate Create(
            Vector target, Vector location, IMaybe<LocationCandiate> parent)
        {
            return 
                new LocationCandiate(
                    location,
                    CalculateMoveCost(parent, location),
                    EstimateRemainingMoveCost(location, target),
                    parent);
        }

        public Vector Location
        {
            get { return _location; }
        }

        public int CurrentCost
        {
            get { return _currentCost; }
        }

        public int RemainingEstimatedCost
        {
            get { return _remainingEstimatedCost; }
        }

        public int OverallCost
        {
            get { return _currentCost + _remainingEstimatedCost; }
        }

        public IMaybe<LocationCandiate> Parent
        {
            get { return _parent; }
        }

        public static int CalculateMoveCost(IMaybe<LocationCandiate> parent, Vector location)
        {
            var parentCost = parent.Select(x => x._currentCost).OrElse(0);

            var moveCost = parent
                .Select(x => (int)(x.Location.DistanceFrom(location) * DistanceMultiplier))
                .OrElse(0);

            return parentCost + moveCost;
        }

        public static int EstimateRemainingMoveCost(Vector locatiobn, Vector target)
        {
            var remainingMove = (locatiobn - target).Abs();

            return (remainingMove.X + remainingMove.Y) * DistanceMultiplier;
        }
    }
}