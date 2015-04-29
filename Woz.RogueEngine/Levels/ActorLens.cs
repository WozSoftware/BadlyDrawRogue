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
using Woz.Lenses;

namespace Woz.RogueEngine.Levels
{
    using IStatisticsStore = IImmutableDictionary<StatisticTypes, int>;
    using IThingStore = IImmutableDictionary<long, Thing>;

    public static class ActorLens
    {
        public static readonly Lens<Actor, long> Id;
        public static readonly Lens<Actor, ActorTypes> ActorType;
        public static readonly Lens<Actor, string> Name;
        public static readonly Lens<Actor, IStatisticsStore> Statistics;
        public static readonly Lens<Actor, HitPoints> HitPoints;
        public static readonly Lens<Actor, IThingStore> Things;

        static ActorLens()
        {
            Id = Lens.Create<Actor, long>(
                actor => actor.Id,
                id => actor => actor.With(id: id));

            ActorType = Lens.Create<Actor, ActorTypes>(
                actor => actor.ActorType,
                actorType => actor => actor.With(actorType: actorType));

            Name = Lens.Create<Actor, string>(
                actor => actor.Name,
                name => actor => actor.With(name: name));

            Statistics = Lens.Create<Actor, IStatisticsStore>(
                actor => actor.Statistics,
                statistics => actor => actor.With(statistics: statistics));

            HitPoints = Lens.Create<Actor, HitPoints>(
                actor => actor.HitPoints,
                hitPoints => actor => actor.With(hitPoints: hitPoints));

            Things = Lens.Create<Actor, IThingStore>(
                actor => actor.Things,
                things => actor => actor.With(things: things));
        }
    }
}