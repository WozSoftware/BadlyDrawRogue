#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Functional.
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

namespace Woz.Functional.Monads.TryMonad
{
    public static class TryLinq
    {
        public static ITry<TResult> Select<T, TResult>(
            this ITry<T> self, Func<T, TResult> selector)
        {
            return self.Map(selector);
        }

        public static ITry<TResult> SelectMany<T, TResult>(
            this ITry<T> self, Func<T, ITry<TResult>> selector)
        {
            return self.FlatMap(selector);
        }

        public static ITry<TResult> SelectMany<T1, T2, TResult>(
            this ITry<T1> self, 
            Func<T1, ITry<T2>> transform, 
            Func<T1, T2, TResult> composer)
        {
            return self.FlatMap(x => 
                    transform(x).FlatMap(y => 
                        composer(x, y).ToSuccess()));
        }
    }
}