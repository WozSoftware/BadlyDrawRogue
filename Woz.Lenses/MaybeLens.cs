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

using System;
using Woz.Monads.MaybeMonad;

namespace Woz.Lenses
{
    public static class MaybeLens<T>
    {
        public static readonly Lens<IMaybe<T>, T> ExpectSome;

        static MaybeLens()
        {
            ExpectSome = new Lens<IMaybe<T>, T>(
                maybe => maybe.OrElseThrow(
                    () => new InvalidOperationException("Some expected")),
                value => maybe => value.ToSome());
        }
    }
}