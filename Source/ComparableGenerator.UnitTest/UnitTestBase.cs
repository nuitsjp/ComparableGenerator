using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace ComparableGenerator.UnitTest
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class UnitTestBase
    {
        [Theory]
        [InlineData(@"
using ComparableGenerator;

    namespace MyNamespace
    {
        public class MyClass
    {
    }
}
")]
        public abstract Task Should_not_be_generated_When_Comparable_and_CompareBy_is_undefined(string source);

        [Theory]
        [InlineData(@"
using System;
using ComparableGenerator;

namespace MyNamespace
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

        public int NotApplicable { get; set; }
    }
}")]
        public abstract Task Should_be_generated_for_class(string source);


        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
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

        public int NotApplicable { get; set; }
    }
}
")]
        public abstract Task Should_be_generated_for_struct(string source);

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public partial struct CompositeObject
    {
        [CompareBy]
        public ClassObject Value { get; set; }
    }

    [ComparableAttribute]
    public partial class ClassObject
    {
        [CompareBy]
        public int Value { get; set; }
    }
}
")]
        public abstract Task Should_be_generated_for_composite(string source);

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public class MyClass
    {
        public int Value { get; set; }
    }
}
")]
        public abstract Task Should_be_error_When_Comparable_is_defined_and_CompareBy_is_undefined_for_class(string source);

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public struct MyClass
    {
        public int Value { get; set; }
    }
}
")]

        public abstract Task Should_be_error_When_Comparable_is_defined_and_CompareBy_is_undefined_for_struct(string source);

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    public class MyClass
    {
        [CompareBy]
        public int Value { get; set; }
    }
}
")]
        public abstract Task Should_be_error_When_Comparable_is_undefined_and_CompareBy_is_defined_for_class(string source);

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    public struct MyClass
    {
        [CompareBy]
        public int Value { get; set; }
    }
}
")]
        public abstract Task Should_be_error_When_Comparable_is_undefined_and_CompareBy_is_defined_for_struct(string source);

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public class MyClass
    {
        [CompareBy]
        public int Value1 { get; set; }

        [CompareBy(Priority = 1)]
        public int Value2;

        public int Value3 { get; set; }

        [CompareBy(Priority = 0)]
        public int Value4;

        [CompareBy(Priority = 1)]
        public int Value5;

        [CompareBy(Priority = 2)]
        public int Value6;
    }
}
")]
        public abstract Task Should_be_error_When_CompareBy_with_same_priority_is_defined_for_class(string source);

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public struct MyClass
    {
        [CompareBy]
        public int Value1 { get; set; }

        [CompareBy(Priority = 1)]
        public int Value2;

        public int Value3 { get; set; }

        [CompareBy(Priority = 0)]
        public int Value4;

        [CompareBy(Priority = 1)]
        public int Value5;

        [CompareBy(Priority = 2)]
        public int Value6;
    }
}
")]
        public abstract Task Should_be_error_When_CompareBy_with_same_priority_is_defined_for_struct(string source);

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public class MyClass
    {
        [CompareBy]
        public object Value { get; set; }
    }
}
")]
        public abstract Task Should_be_error_When_CompareBy_property_does_not_implement_IComparable(string source);

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public struct MyClass
    {
        [CompareBy]
        public object Value;
    }
}
")]
        public abstract Task Should_be_error_When_CompareBy_field_does_not_implement_IComparable(string source);
    }
}