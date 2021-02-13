using System.Threading.Tasks;
using ComparableGenerator.UnitTest.Assertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ComparableGenerator.UnitTest
{
    // ReSharper disable once UnusedMember.Global
    public class SourceGeneratorTest : UnitTestBase
    {
        public override async Task Should_not_be_generated_When_Comparable_and_CompareBy_is_undefined(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }

        public override async Task Should_be_generated_for_class(string source)
        {
            #region Expected
            var expected = CSharpSyntaxTree.ParseText(@"using System;

namespace MyNamespace
{
    public partial class ClassObject : IComparable, IComparable<ClassObject>
    {
        public int CompareTo(object other)
        {
            if (other is null) return 1;

            if (other is ClassObject concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException(""Object is not a MyNamespace.ClassObject."");
        }

        public int CompareTo(ClassObject other)
        {
            if (other is null) return 1;

            int compared;

            compared = Value1.CompareTo(other.Value1);
            if (compared != 0) return compared;

            compared = Value3.CompareTo(other.Value3);
            if (compared != 0) return compared;

            return Value2.CompareTo(other.Value2);
        }
    }
}
");
            #endregion

            await source.RunGenerator()
                .Should().BeGeneratedAsync(expected);
        }

        public override async Task Should_be_generated_for_struct(string source)
        {
            #region Expected
            var expected = CSharpSyntaxTree.ParseText(@"using System;

namespace MyNamespace
{
    public partial struct StructObject : IComparable, IComparable<StructObject>
    {
        public int CompareTo(object other)
        {
            if (other is null) return 1;

            if (other is StructObject concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException(""Object is not a MyNamespace.StructObject."");
        }

        public int CompareTo(StructObject other)
        {
            int compared;

            compared = Value1.CompareTo(other.Value1);
            if (compared != 0) return compared;

            compared = Value3.CompareTo(other.Value3);
            if (compared != 0) return compared;

            return Value2.CompareTo(other.Value2);
        }
    }
}
");
            #endregion

            await source.RunGenerator()
                .Should().BeGeneratedAsync(expected);
        }

        public override async Task Should_be_generated_for_composite(string source)
        {
            #region Expected

            var expected = new []
            {
                CSharpSyntaxTree.ParseText(@"using System;

namespace MyNamespace
{
    public partial struct CompositeObject : IComparable, IComparable<CompositeObject>
    {
        public int CompareTo(object other)
        {
            if (other is null) return 1;

            if (other is CompositeObject concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException(""Object is not a MyNamespace.CompositeObject."");
        }

        public int CompareTo(CompositeObject other)
        {
            int compared;

            return Value.CompareTo(other.Value);
        }
    }
}
"),
                CSharpSyntaxTree.ParseText(@"using System;

namespace MyNamespace
{
    public partial class ClassObject : IComparable, IComparable<ClassObject>
    {
        public int CompareTo(object other)
        {
            if (other is null) return 1;

            if (other is ClassObject concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException(""Object is not a MyNamespace.ClassObject."");
        }

        public int CompareTo(ClassObject other)
        {
            if (other is null) return 1;

            int compared;

            return Value.CompareTo(other.Value);
        }
    }
}
")
            };
            #endregion

            await source.RunGenerator()
                .Should().BeGeneratedAsync(expected);
        }

        public override async Task Should_be_generated_for_nested(string source)
        {
            #region Expected
            var expected = CSharpSyntaxTree.ParseText(@"using System;

namespace GenerateSource
{
    public partial class NestedValueClass : IComparable, IComparable<NestedValueClass>
    {
        public int CompareTo(object other)
        {
            if (other is null) return 1;

            if (other is NestedValueClass concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException(""Object is not a GenerateSource.NestedValueClass."");
        }

        public int CompareTo(NestedValueClass other)
        {
            if (other is null) return 1;

            int compared;

            return Value.CompareTo(other.Value);
        }
    }
}
");
            #endregion

            await source.RunGenerator()
                .Should().BeGeneratedAsync(expected);
        }

        public override async Task Should_be_error_When_Comparable_is_defined_and_CompareBy_is_undefined_for_class(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }

        public override async Task Should_be_error_When_Comparable_is_defined_and_CompareBy_is_undefined_for_struct(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }

        public override async Task Should_be_error_When_Comparable_is_undefined_and_CompareBy_is_defined_for_class(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }

        public override async Task Should_be_error_When_Comparable_is_undefined_and_CompareBy_is_defined_for_struct(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }

        public override async Task Should_be_error_When_CompareBy_with_same_priority_is_defined_for_class(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }

        public override async Task Should_be_error_When_CompareBy_with_same_priority_is_defined_for_struct(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }

        public override async Task Should_be_error_When_CompareBy_property_does_not_implement_IComparable(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }

        public override async Task Should_be_error_When_CompareBy_field_does_not_implement_IComparable(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }
    }
}
