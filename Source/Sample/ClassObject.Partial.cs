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

            // ReSharper disable once JoinDeclarationAndInitializer
            int compared;
            
            compared = MyNamespace.CompareTo.Invoke(Value1, other.Value1);
            if (compared != 0) return compared;

            compared = MyNamespace.CompareTo.Invoke(Value3, other.Value3);
            if (compared != 0) return compared;

            return MyNamespace.CompareTo.Invoke(Value2, other.Value2);
        }
    }
}