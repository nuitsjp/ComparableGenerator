using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace ComparableGenerator.IntegrationTest
{
    public class GenerateComparableTest
    {
        public static IEnumerable<object[]> CompareWith { get; } =
            new List<object[]>
            {
                new object[]{typeof(GenerateSource.ClassObject) },
                new object[]{typeof(GenerateSource.StructObject)},
                new object[]{typeof(GenerateSourceForNotNullable.ClassObject) },
                new object[]{typeof(GenerateSourceForNotNullable.StructObject)},
            };

        [Theory]
        [InlineData(typeof(GenerateSource.NoComparable.ClassObject))]
        [InlineData(typeof(GenerateSource.NoComparable.StructObject))]
        public void Should_not_implement_ICompare_for_CompareAttribute_is_not_defined(Type compareWith)
        {
            var obj = Activator.CreateInstance(compareWith);
            obj.Should().NotBeAssignableTo<IComparable>();
            obj.Should().NotBeAssignableTo(typeof(IComparable<>).MakeGenericType(compareWith));
        }

        [Theory]
        [MemberData(nameof(CompareWith))]
        public void Should_implement_ICompare_for_CompareAttribute_is_defined(Type compareWith)
        {
            var obj = Activator.CreateInstance(compareWith);
            obj.Should().BeAssignableTo<IComparable>();
            obj.Should().BeAssignableTo(typeof(IComparable<>).MakeGenericType(compareWith));
        }

        [Theory]
        [MemberData(nameof(CompareWith))]
        public void Should_return_1_for_CompareTo_by_null_object(Type compareWith)
        {
            var comparable = (IComparable)Activator.CreateInstance(compareWith)!;
            comparable.CompareTo(null).Should().Be(1);
        }

        [Fact]
        public void Should_return_1_for_CompareTo_by_null_concrete_object()
        {
            var comparable = new GenerateSource.ClassObject();
            comparable.CompareTo(null!).Should().Be(1);
        }

        [Fact]
        public void Should_return_minus1_for_self_member_is_null()
        {
            var instance0 = new GenerateSource.CompositeObject { Value = null };
            var instance1 = new GenerateSource.CompositeObject { Value = new GenerateSource.CompositeChildValue { Value1 = 2 } };

            instance0.CompareTo(instance1).Should().Be(-1);
        }

        [Fact]
        public void Should_return_1_for_other_member_is_null()
        {
            var instance0 = new GenerateSource.CompositeObject { Value = new GenerateSource.CompositeChildValue { Value1 = 1 } };
            var instance1 = new GenerateSource.CompositeObject { Value = null };

            instance0.CompareTo(instance1).Should().Be(1);
        }

        [Fact]
        public void Should_return_0_for_both_member_is_null()
        {
            var instance0 = new GenerateSource.CompositeObject { Value = null };
            var instance1 = new GenerateSource.CompositeObject { Value = null };

            instance0.CompareTo(instance1).Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(CompareWith))]
        public void Should_throw_ArgumentException_for_CompareTo_different_type(Type compareWith)
        {
            var comparable = (IComparable)Activator.CreateInstance(compareWith)!;
            var instanceOfDifferentType = string.Empty;
            comparable.Invoking(x => x.CompareTo(instanceOfDifferentType))
                .Should().Throw<ArgumentException>()
                .WithMessage($"Object is not a {compareWith.FullName}.");
        }

        [Theory]
        [MemberData(nameof(CompareWith))]
        public void Should_return_CompareTo_result_of_first_member(Type compareWith)
        {
            var instance0 = (dynamic)Activator.CreateInstance(compareWith)!;
            instance0.Value1 = 1;
            var instance1 = (dynamic)Activator.CreateInstance(compareWith)!;
            instance1.Value1 = 2;

            ((IComparable)instance0).CompareTo((object)instance1)
                .Should().Be(instance0.Value1.CompareTo(instance1.Value1));
        }

        [Theory]
        [MemberData(nameof(CompareWith))]
        public void Should_return_CompareTo_result_of_intermediate_member(Type compareWith)
        {
            var instance0 = (dynamic)Activator.CreateInstance(compareWith)!;
            instance0.Value1 = 0;
            instance0.Value2 = 2;
            instance0.Value3 = 1;
            var instance1 = (dynamic)Activator.CreateInstance(compareWith)!;
            instance1.Value1 = 0;
            instance1.Value2 = 1;
            instance1.Value3 = 2;

            ((IComparable)instance0).CompareTo((object)instance1)
                .Should().Be(instance0.Value3.CompareTo(instance1.Value3));
        }

        [Theory]
        [MemberData(nameof(CompareWith))]
        public void Should_return_CompareTo_result_of_last_member(Type compareWith)
        {
            var instance0 = (dynamic)Activator.CreateInstance(compareWith)!;
            instance0.Value1 = 0;
            instance0.Value2 = 1;
            instance0.Value3 = 0;
            var instance1 = (dynamic)Activator.CreateInstance(compareWith)!;
            instance1.Value1 = 0;
            instance1.Value2 = 2;
            instance1.Value3 = 1;

            ((IComparable)instance0).CompareTo((object)instance1)
                .Should().Be(instance0.Value2.CompareTo(instance1.Value2));
        }

        [Theory]
        [MemberData(nameof(CompareWith))]
        public void Should_return_CompareTo_result_exclude_not_applicable_member(Type compareWith)
        {
            var instance0 = (dynamic)Activator.CreateInstance(compareWith)!;
            instance0.Value1 = 0;
            instance0.Value2 = 0;
            instance0.Value3 = 0;
            instance0.NotApplicable = 1;
            var instance1 = (dynamic)Activator.CreateInstance(compareWith)!;
            instance1.Value1 = 0;
            instance1.Value2 = 0;
            instance1.Value3 = 0;
            instance1.NotApplicable = 2;

            ((IComparable)instance0).CompareTo((object)instance1)
                .Should().Be(0);
        }

        [Fact]
        public void Should_return_CompareTo_result_for_type_with_subclass_of_generated_code_as_member()
        {
            var instance0 = new GenerateSource.CompositeObject {Value = new GenerateSource.CompositeChildValue {Value1 = 1}};
            var instance1 = new GenerateSource.CompositeObject {Value = new GenerateSource.CompositeChildValue { Value1 = 2}};

            instance0.CompareTo(instance1)
                .Should().Be(instance0.Value.Value1.CompareTo(instance1.Value.Value1));
        }

        [Fact]
        public void Should_return_CompareTo_result_for_type_with_subclass_of_IComparable_as_member()
        {
            var instance0 = new GenerateSource.NestedValueClass { Value = new () { Value = 1 } };
            var instance1 = new GenerateSource.NestedValueClass { Value = new() { Value = 2 } };

            instance0.CompareTo(instance1)
                .Should().Be(instance0.Value.Value.CompareTo(instance1.Value.Value));
        }
    }
}
