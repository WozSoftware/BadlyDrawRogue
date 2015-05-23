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
using Woz.Core.Geometry;
using Woz.Linq.Collections;
using Woz.Monads.MaybeMonad;

namespace Woz.PathFinding
{
    // See: http://www.policyalmanac.org/games/aStarTutorial.htm

    // TODO: If performance is an issue use Bin heap for nodes?
    // http://www.codeproject.com/Articles/126751/Priority-queue-in-C-with-the-help-of-heap-data-str

    public static class RouteFinder
    {
        public static IMaybe<Path> FindRoute(
            this Vector start, Vector target,
            Func<Vector, bool> isValidMove)
        {
            return FindRoute(start, target, isValidMove, Maybe<int>.None);
        }

        public static IMaybe<Path> FindRoute(
            this Vector start, Vector target,
            Func<Vector, bool> isValidMove,
            IMaybe<int> breakSize)
        {
            Debug.Assert(isValidMove != null);

            if (start == target)
            {
                return Maybe<Path>.None;
            }

            var lists = RouteFinderLists
                .Create()
                .Open(LocationCandiate.Create(target, start));

            while (lists.HasOpenCandidates)
            {
                // Capture to stop access modified closure
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

                lists = OpenCandidateMoves(
                    lists, currentNode, target, isValidMove);
            }

            return Maybe<Path>.None;
        }

        public static RouteFinderLists OpenCandidateMoves(
            RouteFinderLists lists,
            LocationCandiate currentNode,
            Vector target,
            Func<Vector, bool> isValidMove)
        {
            Func<Vector, bool> isViable =
                move => isValidMove(move) && !lists.IsClosed(move);

            return currentNode.Location 
                .GetValidMoves(isViable)
                .Select(move => LocationCandiate
                    .Create(target, move, currentNode.ToSome()))
                .Aggregate(
                    lists.Close(currentNode),
                    (finderLists, candiate) => finderLists.Open(candiate));
        }

        public static IEnumerable<Vector> GetValidMoves(
            this Vector currentLocation,
            Func<Vector, bool> isValidMove)
        {
            return Directions.FourPoint
                .Select(move => currentLocation + move)
                .Where(isValidMove);
        }

        public static Path BuildActorPath(LocationCandiate targetCandidate)
        {
            // Do not expect one node, would mean start = end
            Debug.Assert(targetCandidate.Parent.HasValue);

            // Pop last location added as it will be the current location
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