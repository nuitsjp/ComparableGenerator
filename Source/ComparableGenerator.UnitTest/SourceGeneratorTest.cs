using System.Threading.Tasks;
using ComparableGenerator.UnitTest.Assertions;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;

namespace ComparableGenerator.UnitTest
{
    public class SourceGeneratorTest : UnitTestBase
    {
        public override async Task Should_not_be_generated_for_CompareAttribute_is_not_defined(string source)
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

        public override async Task Should_not_be_generated_When_not_exists_CompareBy(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }

        public override async Task Should_not_be_generated_When_not_exists_Compare(string source)
        {
            await source.RunGenerator()
                .Should().BeNotGeneratedAsync();
        }
    }
}
