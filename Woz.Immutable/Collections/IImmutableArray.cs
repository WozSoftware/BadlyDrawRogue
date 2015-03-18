#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Immutable.
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
using System.Collections.Generic;
using Woz.Monads.MaybeMonad;

namespace Woz.Immutable.Collections
{
    public interface IImmutableArray<T> : IReadOnlyCollection<T>
    {
        T this[int index] { get; }

        IMaybe<int> IndexOf(Predicate<T> predicate);

        IImmutableArray<T> Set(int index, T item);

        ImmutableArray<T>.Builder ToBuilder();
    }
}