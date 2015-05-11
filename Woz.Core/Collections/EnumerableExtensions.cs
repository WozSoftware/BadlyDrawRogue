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
using System.Collections.Generic;
using System.Diagnostics;
using Woz.Monads.MaybeMonad;

namespace Woz.Core.Collections
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> LinkedListToEnumerable<T>(
            this T rootNode, Func<T, IMaybe<T>> nextNodeSelector)
        {
            Debug.Assert(nextNodeSelector != null);

            var node = rootNode.ToMaybe();
            while (node.HasValue)
            {
                yield return node.Value;
                node = nextNodeSelector(node.Value);
            }
        }

        public static IEnumerable<T> Select<T>(this IEnumerator<T> enumerator)
        {
            Debug.Assert(enumerator!= null);

            return enumerator.Select(x => x);
        }

        public static IEnumerable<TResult> Select<T, TResult>(
            this IEnumerator<T> enumerator, Func<T, TResult> selector)
        {
            Debug.Assert(enumerator != null);
            Debug.Assert(selector != null);

            while (enumerator.MoveNext())
            {
                yield return selector(enumerator.Current);
            }
        }

        public static T MinBy<T, TKey>(
            this IEnumerable<T> self, Func<T, TKey> selector)
        {
            return self.CompareBy(selector, x => x < 0);
        }

        public static T MaxBy<T, TKey>(
            this IEnumerable<T> self, Func<T, TKey> selector)
        {
            return self.CompareBy(selector, x => x > 0);
        }

        private static T CompareBy<T, TKey>(
            this IEnumerable<T> self, 
            Func<T, TKey> selector, 
            Func<int, bool> isBetter)
        {
            Debug.Assert(self != null);
            Debug.Assert(selector != null);
            Debug.Assert(isBetter != null);

            var comparer = Comparer<TKey>.Default;

            using (var enumerator = self.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence has no elements");
                }

                var best = enumerator.Current;
                var bestKey = selector(best);

                while (enumerator.MoveNext())
                {
                    var candidate = enumerator.Current;
                    var candidateKey = selector(candidate);

                    if (isBetter(comparer.Compare(candidateKey, bestKey)))
                    {
                        best = candidate;
                        bestKey = candidateKey;
                    }
                }

                return best;
            }
        }

        public static void ForEach<T>(
            this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self)
            {
                action(item);
            }
        }
    }
}