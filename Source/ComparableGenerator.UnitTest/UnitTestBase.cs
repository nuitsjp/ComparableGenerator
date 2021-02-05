﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace ComparableGenerator.UnitTest
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class UnitTestBase
    {
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
        public abstract Task Should_be_generated_for_class(string inputCompilation);


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
        public abstract Task Should_be_generated_for_struct(string inputCompilation);

        [Theory]
        [InlineData(@"
using ComparableGenerator;

namespace MyNamespace
{
    [Comparable]
    public class MyClass
    {
    }
}
")]
        public abstract Task Should_not_be_generated_When_not_exists_CompareBy(string source);

        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }
}