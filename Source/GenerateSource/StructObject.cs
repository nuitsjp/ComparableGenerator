using ComparableGenerator;

namespace GenerateSource
{
    [Comparable]
    public partial struct StructObject
    {
        [CompareBy]
        public int Value1 { get; set; }

        [CompareBy(Priority = 2)] 
        public int Value2;

        [CompareBy(Priority = 1)]
        public int Value3 { get; set; }

        // ReSharper disable once UnusedMember.Global
        public int NotApplicable { get; set; }
    }
}
