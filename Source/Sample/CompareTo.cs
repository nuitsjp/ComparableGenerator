using System;

namespace MyNamespace
{
    public static class CompareTo
    {
        public static int Invoke<T>(T? self, T? other) where T : IComparable
        {
            if (self is null && other is null) return 0;

            if (self is null) return -1;

            if (other is null) return 1;

            return self.CompareTo(other);
        }
    }
}