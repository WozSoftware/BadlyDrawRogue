using System.Collections.Generic;

namespace Woz.Core.Geometry
{
    public static class Directions
    {
        public static readonly Vector North = Vector.Create(0, 1);
        public static readonly Vector NorthEast = Vector.Create(1, 1);
        public static readonly Vector East = Vector.Create(1, 0);
        public static readonly Vector SouthEast = Vector.Create(1, -1);
        public static readonly Vector South = Vector.Create(0, -1);
        public static readonly Vector SouthWest = Vector.Create(-1, -1);
        public static readonly Vector West = Vector.Create(-1, 0);
        public static readonly Vector NorthWest = Vector.Create(-1, 1);

        public static readonly IEnumerable<Vector> FourPoint =
            new[]
            {
                North, 
                East, 
                South, 
                West
            };

        public static readonly IEnumerable<Vector> EightPoint =
            new[]
            {
                North, 
                NorthEast, 
                East, 
                SouthEast, 
                South, 
                SouthWest, 
                West, 
                NorthWest
            };
    }
}