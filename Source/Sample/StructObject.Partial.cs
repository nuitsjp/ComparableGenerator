using System;

namespace GenerateSource
{
    public partial struct StructObject : IComparable, IComparable<StructObject>
    {
#nullable disable
        public int CompareTo(object other)
#nullable enable
        {
            if (other is null) return 1;

            if (other is StructObject concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException("Object is not a GenerateSource.StructObject.");
        }

        public int CompareTo(StructObject other)
        {
            int compared;

            compared = Value1.CompareTo(other.Value1);
            if (compared != 0) return compared;

            compared = Value3.CompareTo(other.Value3);
            if (compared != 0) return compared;

            return Value2.CompareTo(other.Value2);
        }
    }
}