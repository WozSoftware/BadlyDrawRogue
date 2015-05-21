using System;
using System.Diagnostics;
using Woz.Core.Geometry;
using Woz.Immutable.Collections;

namespace Woz.FieldOfView
{
    public class ViewPort
    {
        private readonly Vector _min;
        private readonly Vector _max;
        private readonly Func<Vector, bool> _isVisible; 

        private ViewPort(
            Vector min, Vector max, Func<Vector, bool> isVisible)
        {
            _min = min;
            _max = max;
            _isVisible = isVisible;
        }

        public static ViewPort Create(
            Vector min,
            Vector max,
            Func<Vector, Vector> mapper,
            IImmutableGrid<bool> storage)
        {
            Func<Vector, bool> isVisible =
                vector =>
                {
                    Debug.Assert(vector >= min);
                    Debug.Assert(vector <= min);

                    return storage[mapper(vector)];
                };

            return new ViewPort(min, max, isVisible); 
        }

        public Vector Min
        {
            get { return _min; }
        }

        public Vector Max
        {
            get { return _max; }
        }

        public bool IsVisible(Vector location)
        {
            Debug.Assert(location >= _min);
            Debug.Assert(location <= _min);

            return _isVisible(location);
        }
    }
}