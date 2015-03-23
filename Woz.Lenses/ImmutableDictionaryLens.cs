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
using System.Collections.Immutable;
using Woz.Monads.MaybeMonad;

namespace Woz.Lenses
{
    public static class ImmutableDictionaryLens
    {
        public static 
            Lens<IImmutableDictionary<TKey, TValue>, TValue> 
            ByKey<TKey, TValue>(TKey key)
        {
            return 
                Lens.Create<IImmutableDictionary<TKey, TValue>, TValue>(
                    dictionary => dictionary[key],
                    value => dictionary => dictionary.SetItem(key, value));
        }

        public static Lens<TType, TValue> 
            ByKey<TType, TKey, TValue>(
                this Lens<TType, IImmutableDictionary<TKey, TValue>> lens,
                TKey key)
        {
            return lens.With(ByKey<TKey, TValue>(key));
        }

        public static 
            Lens<IImmutableDictionary<TKey, TValue>, IMaybe<TValue>> 
            Lookup<TKey, TValue>(TKey key)
        {
            return 
                Lens.Create<IImmutableDictionary<TKey, TValue>, IMaybe<TValue>>(
                    dictionary => dictionary.Lookup(key),
                    value => dictionary =>
                    {
                        return value
                            .Select(x => dictionary.SetItem(key, x))
                            .OrElse(() => dictionary.Remove(key));
                    });
        }

        public static Lens<TType, IMaybe<TValue>> 
            Lookup<TType, TKey, TValue>(
                this Lens<TType, IImmutableDictionary<TKey, TValue>> lens,
                TKey key)
        {
            return lens.With(Lookup<TKey, TValue>(key));
        }

        public static
            Lens<IImmutableDictionary<TKey, TValue>, TValue>
            Lookup<TKey, TValue>(TKey key, Func<TKey, TValue> defaultFactory)
        {
            return
                Lens.Create<IImmutableDictionary<TKey, TValue>, TValue>(
                    dictionary => dictionary.Lookup(key).OrElse(defaultFactory(key)),
                    // ReSharper disable once ImplicitlyCapturedClosure
                    value => dictionary => dictionary.SetItem(key, value));
        }

        public static Lens<TEntity, TValue>
            Lookup<TEntity, TKey, TValue>(
                this Lens<TEntity, IImmutableDictionary<TKey, TValue>> lens,
                TKey key, Func<TKey, TValue> defaultFactory)
        {
            return lens.With(Lookup(key, defaultFactory));
        }

        public static TEntity RemoveByKey<TEntity, TKey, TValue>(
            this TEntity instance,
            Lens<TEntity, IImmutableDictionary<TKey, TValue>> lens,
            TKey key)
        {
            return instance.Set(lens.Lookup(key), Maybe<TValue>.None);
        }
    }
}