using ComparableGenerator;

namespace GenerateSourceForNotNullable
{
    [Comparable]
    internal partial class CompositeObject
    {
        [CompareBy]
        public CompositeChildValue Value { get; set; }
    }

    public class CompositeChildValue : ClassObject { }
}