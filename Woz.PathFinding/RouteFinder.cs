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
using System.Diagnostics.CodeAnalysis;
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
        // ImplicitlyCapturedClosure suppressed as the lambdas do not escape 
        // the function so there are no knock on GC issues to deal with
        [SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
        public static IMaybe<Path> FindRoute(
            Vector start, Vector target,
            Func<Vector, bool> isValidWorldMove,
            IEnumerable<Vector> moveVectors)
        {
            Debug.Assert(isValidWorldMove != null);
            Debug.Assert(moveVectors != null);

            if (!isValidWorldMove(target))
            {
                return Maybe<Path>.None;
            }

            var validMoveVectors = moveVectors.ToArray();
            var closeList = new Dictionary<Vector, LocationCandiate>();
            var openList = new Dictionary<Vector, LocationCandiate>();

            Func<Vector, bool> isValidMove =
                vector => !closeList.ContainsKey(vector) &&
                          isValidWorldMove(vector);

            Action<LocationCandiate> closeCandidate =
                toClose =>
                {
                    if (openList.Remove(toClose.Location))
                    {
                        closeList[toClose.Location] = toClose;
                    }
                };
            
            var candidate = LocationCandiate.Create(start).ToSome();
            while (candidate.Select(x => x.Location != target).OrElse(false))
            {
                var currentCandidate = candidate;
                currentCandidate.Do(closeCandidate);

                validMoveVectors
                    .GetValidMoves(candidate.Value.Location, isValidMove)
                    .Select(
                        move =>
                            BuildMoveLocationCandidate(
                                currentCandidate,
                                openList.Lookup(move),
                                move))
                    .WhereHasValue()
                    .ForEach(
                        newCandidate =>
                        {
                            openList[newCandidate.Location] = newCandidate;
                        });

                candidate = openList.Values.OrderBy(x => x.Cost).FirstMaybe();
            }

            return candidate.Select(BuildActorPath);
        }

        public static IEnumerable<Vector> GetValidMoves(
            this IEnumerable<Vector> moveVectors,
            Vector currentLocation,
            Func<Vector, bool> isValidMove)
        {
            return moveVectors
                .Select(currentLocation.Add)
                .Where(isValidMove);
        }

        public static IMaybe<LocationCandiate> BuildMoveLocationCandidate(
            IMaybe<LocationCandiate> currentCandidate,
            IMaybe<LocationCandiate> oldMoveLocationCandidate,
            Vector moveLocation)
        {
            var newMoveLocationCandidate = LocationCandiate
                .Create(moveLocation, currentCandidate);

            return oldMoveLocationCandidate
                .Select(x => x.Cost > newMoveLocationCandidate.Cost)
                .OrElse(true) 
                ? newMoveLocationCandidate.ToSome() 
                : Maybe<LocationCandiate>.None;
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