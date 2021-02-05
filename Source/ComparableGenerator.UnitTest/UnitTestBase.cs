using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace ComparableGenerator.UnitTest
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class UnitTestBase
    {
        public static IEnumerable<object[]> Input_to_Should_be_generated_for_class { get; } =
            new List<object[]>
            {
                new object[]{ CreateCompilation(@"
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
}") }
            };

        [Theory]
        [MemberData(nameof(Input_to_Should_be_generated_for_class))]
        public abstract void Should_be_generated_for_class(Compilation inputCompilation);


        public static IEnumerable<object[]> Input_to_Should_be_generated_for_struct { get; } =
            new List<object[]>
            {
                new object[]{ CreateCompilation(@"
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
") }
            };

        [Theory]
        [MemberData(nameof(Input_to_Should_be_generated_for_struct))]
        public abstract void Should_be_generated_for_struct(Compilation inputCompilation);

        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }
}