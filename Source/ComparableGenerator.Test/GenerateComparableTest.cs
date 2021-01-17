using FluentAssertions;
using GenerateSource;
using Xunit;

namespace ComparableGenerator.Test
{
    public class GenerateComparableTest
    {
        [Fact]
        public void Compare_by_concrete_type_by_first_member()
        {
            var obj1 = new StructObject {Value1 = 1};
            var obj2 = new StructObject {Value1 = 2};

            obj1.CompareTo(obj2).Should().Be(obj1.Value1.CompareTo(obj2.Value1));
        }

        [Fact]
        public void Compare_by_concrete_type_by_last_member()
        {
            var obj1 = new StructObject {Value1 = 0, Value2 = 0, Value3 = 1};
            var obj2 = new StructObject {Value1 = 0, Value2 = 0, Value3 = 2};

            obj1.CompareTo(obj2).Should().Be(obj1.Value3.CompareTo(obj2.Value3));
        }
    }
}
