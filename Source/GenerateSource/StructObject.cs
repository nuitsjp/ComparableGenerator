using System;
using ComparableGenerator;

namespace GenerateSource
{
    [Comparable]
    public partial struct StructObject// : IComparable
    {
        [CompareBy]
        public int Value1 { get; set; }

        [CompareBy(Priority = 2)]
        public int Value2 { get; set; }

        [CompareBy(Priority = 1)]
        public int Value3 { get; set; }

        public int NotApplicable { get; set; }

        //public int CompareTo(object obj)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
