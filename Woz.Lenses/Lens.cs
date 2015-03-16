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
using System.Diagnostics;

namespace Woz.Lenses
{
    public static class Lens
    {
        public static Lens<TType, TValue> Create<TType, TValue>(
            Func<TType, TValue> get,
            Func<TValue, Func<TType, TType>> set)
        {
            return new Lens<TType, TValue>(get, set);
        }

        public static TValue Get<TType, TValue>(
            this TType instance,
            Lens<TType, TValue> lens)
        {
            return lens.Get(instance);
        }

        public static TType Set<TType, TValue>(
            this TType instance,
            Lens<TType, TValue> lens,
            TValue value)
        {
            return lens.Set(value)(instance);
        }
    }

    public class Lens<TType, TValue>
    {
        private readonly Func<TType, TValue> _get;
        private readonly Func<TValue, Func<TType, TType>> _set;

        internal Lens(
            Func<TType, TValue> get, 
            Func<TValue, Func<TType, TType>> set)
        {
            Debug.Assert(get != null);
            Debug.Assert(set != null);

            _get = get;
            _set = set;
        }

        public Func<TType, TValue> Get
        {
            get { return _get; }
        }

        public Func<TValue, Func<TType, TType>> Set
        {
            get { return _set; }
        }
    
        private Func<TType, TType> Modify(Func<TValue, TValue> updater)
        {
            Debug.Assert(updater != null);

            return root => Set(updater(Get(root)))(root);
        }

        public Lens<TType, TChildValue> With<TChildValue>(
            Lens<TValue, TChildValue> childLens)
        {
            Debug.Assert(childLens != null);

            return new Lens<TType, TChildValue>(
                root => childLens.Get(Get(root)),
                childValue => Modify(childLens.Set(childValue)));
        }
    }
}