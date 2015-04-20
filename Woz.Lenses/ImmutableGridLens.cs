#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Lenses.
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

using Woz.Core.Coordinates;
using Woz.Immutable.Collections;

namespace Woz.Lenses
{
    public static class ImmutableGridLens
    {
        public static Lens<IImmutableGrid<TValue>, TValue> 
            Location<TValue>(Coordinate location)
        {
            return Location<TValue>(location.X, location.Y);
        }

        public static Lens<TEntity, TValue> Location<TEntity, TValue>(
            this Lens<TEntity, IImmutableGrid<TValue>> self, Coordinate location)
        {
            return self.With(Location<TValue>(location.X, location.Y));
        }

        public static Lens<IImmutableGrid<TValue>, TValue>
            Location<TValue>(int x, int y)
        {
            return Lens
                .Create<IImmutableGrid<TValue>, TValue>(
                    grid => grid[x, y],
                    value => grid => grid.Set(x, y, value));
        }

        public static Lens<TEntity, TValue> Location<TEntity, TValue>(
                this Lens<TEntity, IImmutableGrid<TValue>> self, int x, int y)
        {
            return self.With(Location<TValue>(x, y));
        }
    }
}