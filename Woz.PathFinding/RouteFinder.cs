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
using Woz.Monads.MaybeMonad;

namespace Woz.PathFinding
{
    // See: http://www.policyalmanac.org/games/aStarTutorial.htm

    // TODO: If performance is an issue use Bin heap for nodes?
    // http://www.codeproject.com/Articles/126751/Priority-queue-in-C-with-the-help-of-heap-data-str

    public class RouteFinder
    {
        public IMaybe<Path> FindRoute(
            Vector start, Vector target,
            Func<Vector, bool> isValidLocation,
            IEnumerable<Vector> moveVectors)
        {
            Debug.Assert(isValidLocation != null);

            var openList = new Dictionary<Vector, RouteCandidate>();
            var closeList = new Dictionary<Vector, RouteCandidate>();
            var validMoveVectors = moveVectors.ToArray();

            var candidate = isValidLocation(target)
                ? RouteCandidate.Create(start, target).ToSome()
                : Maybe<RouteCandidate>.None;

            while (candidate.Select(x => x.Location != target).OrElse(false))
            {
                var candidateLocation = candidate.Value.Location;

                if (openList.Remove(candidateLocation))
                {
                    closeList[candidateLocation] = candidate.Value;
                }

                foreach (var directionVector in validMoveVectors)
                {
                    var newCandidateLocation = 
                        candidateLocation.Add(directionVector);

                    if (closeList.ContainsKey(newCandidateLocation) ||
                        !isValidLocation(newCandidateLocation))
                    {
                        continue;
                    }

                    var existingCandidate = 
                        openList.Lookup(newCandidateLocation);

                    var newCandidate = RouteCandidate
                        .Create(candidate, newCandidateLocation, target);

                    if (existingCandidate.Select(x => x.Cost).OrElse(0) > newCandidate.Cost)
                    {
                        openList[newCandidateLocation] = newCandidate;
                    }
                }

                candidate = openList.Values.OrderBy(x => x.Cost).FirstMaybe();
            }

            return candidate.Select(x => BuildActorPath(target, x));
        }

        private static Path BuildActorPath(Vector target, RouteCandidate currentCandidate)
        {
            var path = ImmutableStack<Vector>.Empty;
            var pathEntry = currentCandidate;
            while (pathEntry.Parent.HasValue)
            {
                path = path.Push(pathEntry.Location);
                pathEntry = pathEntry.Parent.Value;
            }

            return Path.Create(target, path);
        }
    }
}