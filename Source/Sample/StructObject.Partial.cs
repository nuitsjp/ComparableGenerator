﻿using System;

namespace MyNamespace
{
    public partial struct StructObject : IComparable, IComparable<StructObject>
    {
        public int CompareTo(object other)
        {
            if (other is null) return 1;

            if (other is ClassObject classObject)
            {
                return CompareTo(classObject);
            }

            throw new ArgumentException("Object is not a MyNamespace.StructObject.");
        }

        public int CompareTo(StructObject other)
        {
            var compared = Value1.CompareTo(other.Value1);
            if (compared != 0) return compared;

            compared = Value3.CompareTo(other.Value3);
            if (compared != 0) return compared;

            return Value2.CompareTo(other.Value2);
        }
    }
}