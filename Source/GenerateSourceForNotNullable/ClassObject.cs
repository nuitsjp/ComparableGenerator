using ComparableGenerator;

namespace GenerateSourceForNotNullable
{
    [Comparable]
    public partial class ClassObject
    {
        [CompareBy]
        public int Value1 { get; set; }

        [CompareBy(Priority = 2)]
        public int Value2;

        [CompareBy(Priority = 1)]
        public int Value3 { get; set; }

        // ReSharper disable once UnusedMember.Global
        public object NotApplicable { get; set; }
    }
}