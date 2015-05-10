using System.Collections.Immutable;
using System.Linq;
using Woz.Core.Geometry;
using Woz.Monads.MaybeMonad;

namespace Woz.PathFinding
{
    public class RouteFinderLists
    {
        private readonly IImmutableDictionary<Vector, LocationCandiate> _openList;
        private readonly IImmutableDictionary<Vector, LocationCandiate> _closedList;

        private RouteFinderLists(
            IImmutableDictionary<Vector, LocationCandiate> openList, 
            IImmutableDictionary<Vector, LocationCandiate> closedList)
        {
            _openList = openList;
            _closedList = closedList;
        }

        public static RouteFinderLists Create()
        {
            return new RouteFinderLists(
                ImmutableDictionary<Vector, LocationCandiate>.Empty,
                ImmutableDictionary<Vector, LocationCandiate>.Empty);
        }

        public IImmutableDictionary<Vector, LocationCandiate> OpenList
        {
            get { return _openList; }
        }

        public IImmutableDictionary<Vector, LocationCandiate> ClosedList
        {
            get { return _closedList; }
        }

        public bool HasOpenCandidates
        {
            get { return _openList.Any(); }
        }

        public LocationCandiate NextOpenCandiate
        {
            get
            {
                return _openList
                    .Select(x => x.Value)
                    .OrderBy(x => x.OverallCost)
                    .First();
            }    
        }

        public bool IsClosed(Vector location)
        {
            return _closedList.ContainsKey(location);
        }

        public RouteFinderLists Open(LocationCandiate candiate)
        {
            var existing = _openList.Lookup(candiate.Location);
            return existing
                .Select(x => x.CurrentCost < candiate.CurrentCost)
                .OrElse(false)
                ? this
                : new RouteFinderLists(
                    _openList.SetItem(candiate.Location, candiate),
                    _closedList);
        }

        public RouteFinderLists Close(LocationCandiate candidate)
        {
            return new RouteFinderLists(
                _openList.Remove(candidate.Location),
                _closedList.SetItem(candidate.Location, candidate));
        }
    }
}