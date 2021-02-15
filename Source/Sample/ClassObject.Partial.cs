using System;

namespace MyNamespace
{
    public partial class ClassObject : IComparable, IComparable<ClassObject>
    {
#nullable disable
        public int CompareTo(object other)
#nullable enable
        {
            if (other is null) return 1;

            if (other is ClassObject concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException("Object is not a MyNamespace.ClassObject.");
        }

#nullable disable
        public int CompareTo(ClassObject other)
#nullable enable
        {
            if (other is null) return 1;

            static int LocalCompareTo<T>(T? left, T? right) where T : IComparable
            {
                if (left is null && right is null) return 0;

                if (left is null) return -1;

                if (right is null) return 1;

                return left.CompareTo(right);
            }

            // ReSharper disable once JoinDeclarationAndInitializer
            int compared;
            
            compared = LocalCompareTo(Value1, other.Value1);
            if (compared != 0) return compared;

            compared = LocalCompareTo(Value3, other.Value3);
            if (compared != 0) return compared;

            return LocalCompareTo(Value2, other.Value2);
        }
    }
}