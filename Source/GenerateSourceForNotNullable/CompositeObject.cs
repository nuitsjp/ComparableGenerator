using ComparableGenerator;

namespace GenerateSourceForNotNullable
{
    [Comparable]
    public partial class CompositeObject
    {
        [CompareBy]
        public CompositeChildValue Value { get; set; }
    }

    public class CompositeChildValue : ClassObject { }
}