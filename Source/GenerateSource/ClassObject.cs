using System;

namespace GenerateSource
{
    public class ClassObject : IComparable<ClassObject>
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public int Value3 { get; set; }

        public int CompareTo(ClassObject? other)
        {
            if (other is null) return 1;

            int compared;

            compared = Value1.CompareTo(other.Value1);
            if (compared != 0) return compared;

            compared = Value2.CompareTo(other.Value2);
            if (compared != 0) return compared;

            compared = Value3.CompareTo(other.Value3);
            return compared;
        }
    }
}