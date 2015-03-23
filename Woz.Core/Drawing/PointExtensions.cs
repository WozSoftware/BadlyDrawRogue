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

using System;
using System.Drawing;

namespace Woz.Core.Drawing
{
    public static class PointExtensions
    {
        public static bool GreaterThan(this Point self, Point minimum)
        {
            return self.X > minimum.X && self.Y > minimum.Y;
        }

        public static bool GreaterOrEqualTo(this Point self, Point minimum)
        {
            return self.X >= minimum.X && self.Y >= minimum.Y;
        }

        public static bool LessThan(this Point self, Point maximum)
        {
            return self.X < maximum.X && self.Y < maximum.Y;
        }

        public static bool LessOrEqualTo(this Point self, Point maximum)
        {
            return self.X <= maximum.X && self.Y <= maximum.Y;
        }

        public static bool Within(this Point self, Point minimum, Point maximum)
        {
            return self.GreaterOrEqualTo(minimum) && self.LessOrEqualTo(maximum);
        }

        public static double DistanceFrom(this Point self, Point target)
        {
            var diffX = Math.Abs(self.X - target.X);
            var diffY = Math.Abs(self.Y - target.Y);

            return Math.Sqrt((diffX * diffX) + (diffY * diffY));
        }

    }
}