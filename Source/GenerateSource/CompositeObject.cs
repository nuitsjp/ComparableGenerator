using ComparableGenerator;

namespace GenerateSource
{
    [Comparable]
    public partial class CompositeObject
    {
        [CompareBy]
        public ClassObject Value { get; set; }
    }
}