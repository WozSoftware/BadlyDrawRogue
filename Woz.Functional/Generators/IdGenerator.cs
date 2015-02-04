using System;

namespace Woz.Functional.Generators
{
    public static class IdGenerator
    {
        public static Func<long> Build()
        {
            long id = 1;
            return () => id++;
        }
    }
}