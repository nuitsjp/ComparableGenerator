using System.Threading.Tasks;
using ComparableGenerator.UnitTest.Assertions;
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

        public override async Task Should_not_be_generated_When_ComparableAttribute_is_not_included(string source)
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
#nullable disable
        public int CompareTo(object other)
#nullable enable
        {
            if (other is null) return 1;

            if (other is ClassObject concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException(""Object is not a MyNamespace.ClassObject."");
        }

#nullable disable
        public int CompareTo(ClassObject other)
#nullable enable
        {
            if (other is null) return 1;

            static int LocalCompareTo<T>(T? left, T? right) where T : IComparable
            {
                if (left is null && right is null) return 0;

                if (left is null) return -1;

                if (right is null) return 1;

                return left.CompareTo(right);
            }

            int compared;

            compared = LocalCompareTo(Value1, other.Value1);
            if (compared != 0) return compared;

            compared = LocalCompareTo(Value3, other.Value3);
            if (compared != 0) return compared;

            return LocalCompareTo(Value2, other.Value2);
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
#nullable disable
        public int CompareTo(object other)
#nullable enable
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
            static int LocalCompareTo<T>(T? left, T? right) where T : IComparable
            {
                if (left is null && right is null) return 0;

                if (left is null) return -1;

                if (right is null) return 1;

                return left.CompareTo(right);
            }

            int compared;

            compared = LocalCompareTo(Value1, other.Value1);
            if (compared != 0) return compared;

            compared = LocalCompareTo(Value3, other.Value3);
            if (compared != 0) return compared;

            return LocalCompareTo(Value2, other.Value2);
        }
    }
}
");
            #endregion

            await source.RunGenerator()
                .Should().BeGeneratedAsync(expected);
        }

        public override async Task Should_be_generated_for_type_with_subclass_of_generated_code_as_member(string source)
        {
            #region Expected

            var expected = new []
            {
                CSharpSyntaxTree.ParseText(@"using System;

namespace MyNamespace
{
    public partial struct CompositeObject : IComparable, IComparable<CompositeObject>
    {
#nullable disable
        public int CompareTo(object other)
#nullable enable
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
            static int LocalCompareTo<T>(T? left, T? right) where T : IComparable
            {
                if (left is null && right is null) return 0;

                if (left is null) return -1;

                if (right is null) return 1;

                return left.CompareTo(right);
            }

            return LocalCompareTo(Value, other.Value);
        }
    }
}
"),
                CSharpSyntaxTree.ParseText(@"using System;

namespace MyNamespace
{
    public partial class ClassObject : IComparable, IComparable<ClassObject>
    {
#nullable disable
        public int CompareTo(object other)
#nullable enable
        {
            if (other is null) return 1;

            if (other is ClassObject concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException(""Object is not a MyNamespace.ClassObject."");
        }

#nullable disable
        public int CompareTo(ClassObject other)
#nullable enable
        {
            if (other is null) return 1;

            static int LocalCompareTo<T>(T? left, T? right) where T : IComparable
            {
                if (left is null && right is null) return 0;

                if (left is null) return -1;

                if (right is null) return 1;

                return left.CompareTo(right);
            }

            return LocalCompareTo(Value, other.Value);
        }
    }
}
")
            };
            #endregion

            await source.RunGenerator()
                .Should().BeGeneratedAsync(expected);
        }

        public override async Task Should_be_generated_for_type_with_subclass_of_IComparable_as_member(string source)
        {
            #region Expected
            var expected = CSharpSyntaxTree.ParseText(@"using System;

namespace GenerateSource
{
    public partial class NestedValueClass : IComparable, IComparable<NestedValueClass>
    {
#nullable disable
        public int CompareTo(object other)
#nullable enable
        {
            if (other is null) return 1;

            if (other is NestedValueClass concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException(""Object is not a GenerateSource.NestedValueClass."");
        }

#nullable disable
        public int CompareTo(NestedValueClass other)
#nullable enable
        {
            if (other is null) return 1;

            static int LocalCompareTo<T>(T? left, T? right) where T : IComparable
            {
                if (left is null && right is null) return 0;

                if (left is null) return -1;

                if (right is null) return 1;

                return left.CompareTo(right);
            }

            return LocalCompareTo(Value, other.Value);
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

        public override async Task Should_be_error_When_multiple_variables_field(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }
    }
}
