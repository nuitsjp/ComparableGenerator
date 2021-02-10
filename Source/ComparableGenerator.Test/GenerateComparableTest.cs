using System;
using System.Collections.Generic;
using FluentAssertions;
using GenerateSource;
using Xunit;

namespace ComparableGenerator.Test
{
    public class GenerateComparableTest
    {
        public static IEnumerable<object[]> CompareWith { get; } =
            new List<object[]>
            {
                new object[]{typeof(ClassObject) },
                new object[]{typeof(StructObject)}
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
            var comparable = new ClassObject();
            comparable.CompareTo(null!).Should().Be(1);
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
    }
}
