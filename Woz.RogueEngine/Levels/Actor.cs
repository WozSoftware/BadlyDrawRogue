#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.RogueEngine.
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
        private readonly long _id;
        private readonly ActorTypes _actorType;
        private readonly string _name;
        private readonly IStatisticsStore _statistics;
        private readonly HitPoints _hitPoints;
        private readonly IThingStore _things;

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

            _id = id;
            _actorType = actorType;
            _name = name;
            _statistics = statistics;
            _hitPoints = hitPoints;
            _things = things;
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

        public long Id
        {
            get { return _id; }
        }

        public ActorTypes ActorType
        {
            get { return _actorType; }
        }

        public string Name
        {
            get { return _name; }
        }

        public IStatisticsStore Statistics
        {
            get { return _statistics; }
        }

        public HitPoints HitPoints
        {
            get { return _hitPoints; }
        }

        public IThingStore Things
        {
            get { return _things; }
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
                        id ?? _id,
                        actorType ?? _actorType,
                        name ?? _name,
                        statistics ?? _statistics,
                        hitPoints ?? _hitPoints,
                        things ?? _things)
                    : this;
        }
    }
}