#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.PathFinding.
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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Woz.Core.Collections;
using Woz.Core.Geometry;
using Woz.Monads.MaybeMonad;

namespace Woz.PathFinding
{
    // See: http://www.policyalmanac.org/games/aStarTutorial.htm

    // TODO: If performance is an issue use Bin heap for nodes?
    // http://www.codeproject.com/Articles/126751/Priority-queue-in-C-with-the-help-of-heap-data-str

    public static class RouteFinder
    {
        public static IMaybe<Path> FindRoute(
            Vector start, Vector target,
            Func<Vector, bool> isValidMove)
        {
            return FindRoute(start, target, isValidMove, Maybe<int>.None);
        }

        public static IMaybe<Path> FindRoute(
            Vector start, Vector target,
            Func<Vector, bool> isValidMove,
            IMaybe<int> breakSize)
        {
            Debug.Assert(isValidMove != null);

            var lists = RouteFinderLists
                .Create()
                .Open(LocationCandiate.Create(target, start));

            while (lists.HasOpenCandidates)
            {
                var closedList = lists.ClosedList;
                if (breakSize.Select(x => x < closedList.Count).OrElse(false))
                {
                    break;
                }

                var currentNode = lists.NextOpenCandiate;

                if (currentNode.Location == target)
                {
                    return BuildActorPath(currentNode).ToSome();
                }

                // ReSharper disable once PossibleMultipleEnumeration
                lists = Directions.FourPoint
                    .GetValidMoves(currentNode.Location, isValidMove)
                    .Where(move => !lists.IsClosed(move))
                    .Select(move => LocationCandiate
                        .Create(target, move, currentNode.ToSome()))
                    .Aggregate(
                        lists.Close(currentNode),
                        (finderLists, candiate) => finderLists.Open(candiate));
            }

            return Maybe<Path>.None;
        }

        public static IEnumerable<Vector> GetValidMoves(
            this IEnumerable<Vector> moveVectors,
            Vector currentLocation,
            Func<Vector, bool> isValidMove)
        {
            return moveVectors
                .Select(move => currentLocation + move)
                .Where(isValidMove);
        }

        public static Path BuildActorPath(LocationCandiate targetCandidate)
        {
            // Pop last location added as itwill be the current location
            return Path.Create(
                targetCandidate.Location,
                targetCandidate
                    .LinkedListToEnumerable(node => node.Parent)
                    .Aggregate(
                        ImmutableStack<Vector>.Empty,
                        (stack, candidate) => stack.Push(candidate.Location))
                    .Pop());
        }
    }
}