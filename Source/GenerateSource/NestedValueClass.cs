using System;
using ComparableGenerator;

namespace GenerateSource
{
    [Comparable]
    public partial class NestedValueClass
    {
        [CompareBy]
        public ChildValue? Value;
    }

    public class ChildValue : ParentValue
    {

    }

    public class ParentValue : IComparable, IComparable<ParentValue>
    {
        public int Value { get; set; }
        public int CompareTo(object? other)
        {
            if (other is null) return 1;

            if (other is ParentValue concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException("Object is not a GenerateSource.ParentValue.");
        }

        public int CompareTo(ParentValue? other)
        {
            if (other is null) return 1;

            return Value.CompareTo(other.Value);
        }
    }
}