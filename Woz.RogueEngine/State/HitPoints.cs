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

using System.Diagnostics;

namespace Woz.RogueEngine.State
{
    public sealed class HitPoints
    {
        private readonly int _maximum;
        private readonly int _current;

        private HitPoints(int maximum, int current)
        {
            Debug.Assert(maximum > 0);
            Debug.Assert(current > 0);
            Debug.Assert(maximum >= current);

            _maximum = maximum;
            _current = current;
        }

        public static HitPoints Create(int maximum, int current)
        {
            return new HitPoints(maximum, current);
        }

        public int Maximum
        {
            get { return _maximum; }
        }

        public int Current
        {
            get { return _current; }
        }

        public HitPoints With(int? maximum = null, int? current = null)
        {
            return maximum == null && current == null
                ? this
                : new HitPoints(maximum ?? _maximum, current ?? _current);
        }
    }
}