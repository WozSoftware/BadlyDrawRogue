#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.RogueEngine.
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

using System;
using Woz.Core.Geometry;
using Woz.FieldOfView;
using Woz.RogueEngine.Levels;
using Woz.RogueEngine.Validators;

namespace Woz.RogueEngine.AI
{
    public static class LineOfSight
    {
        public const int VisibleRegionRadius = 8;

        public static bool CanSee(
            this Level level, Vector location, Vector target)
        {
            return location.CanSee(
                target,
                toTest => !level.BlocksLineOfSight(toTest).IsValid);
        }

        public static Func<Vector, bool> CalculateVisibleRegion(
            this Level level,
            Vector location,
            int radius)
        {
            return location.CalculateVisibleRegion(
                VisibleRegionRadius,
                toTest => !level.BlocksLineOfSight(toTest).IsValid);
        }
    }
}