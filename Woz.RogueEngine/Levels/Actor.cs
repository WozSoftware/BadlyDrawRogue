#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.RoqueEngine.
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

using System.Collections.Immutable;
using System.Diagnostics;

namespace Woz.RogueEngine.Levels
{
    using IStatisticsStore = IImmutableDictionary<StatisticTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    public sealed class Actor
    {
        public readonly long Id;
        public readonly ActorTypes ActorType;
        public readonly string Name;
        public readonly IStatisticsStore Statistics;
        public readonly HitPoints HitPoints;
        public readonly IThingStore Things;

        private Actor(
            long id,
            ActorTypes actorType,
            string name,
            IStatisticsStore statistics,
            HitPoints hitPoints,
            IThingStore things)
        {
            Debug.Assert(name != null);
            Debug.Assert(statistics != null);
            Debug.Assert(hitPoints != null);
            Debug.Assert(things != null);

            Id = id;
            ActorType = actorType;
            Name = name;
            Statistics = statistics;
            HitPoints = hitPoints;
            Things = things;
        }

        public static Actor Create(
            long id,
            ActorTypes actorType,
            string name,
            HitPoints hitPoints)
        {
            return
                new Actor(
                    id,
                    actorType,
                    name,
                    ImmutableDictionary<StatisticTypes, int>.Empty,
                    hitPoints,
                    ImmutableDictionary<long, Thing>.Empty);
        }

        public static Actor Create(
            long id,
            ActorTypes actorType,
            string name,
            IStatisticsStore statistics,
            HitPoints hitPoints,
            IThingStore things)
        {
            return 
                new Actor(
                    id, 
                    actorType, 
                    name, 
                    statistics, 
                    hitPoints, 
                    things);
        }

        public Actor With(
            long? id = null,
            ActorTypes? actorType = null,
            string name = null,
            IStatisticsStore statistics = null,
            HitPoints hitPoints = null,
            IThingStore things = null)
        {
            return
                id.HasValue ||
                actorType.HasValue ||
                name != null ||
                statistics != null ||
                hitPoints != null ||
                things != null
                    ? new Actor(
                        id ?? Id,
                        actorType ?? ActorType,
                        name ?? Name,
                        statistics ?? Statistics,
                        hitPoints ?? HitPoints,
                        things ?? Things)
                    : this;
        }
    }
}