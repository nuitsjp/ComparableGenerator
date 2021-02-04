using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Xunit.Sdk;

namespace ComparableGenerator.UnitTest
{
    public class GenerateComparableTest
    {
        [Fact]
        public async Task Should_be_generated_for_class()
        {
            var inputCompilation = CreateCompilation(@"
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
}");

            RunGenerator(inputCompilation, out var outputCompilation, out var diagnostics);


            diagnostics.Should().BeEmpty();

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
            outputCompilation.SyntaxTrees
                .Should().HaveCount(2)
                .And.Subject.Last()
                    .Should().Be(expected);
        }

        [Fact]
        public void Should_be_generated_for_struct()
        {
            var inputCompilation = CreateCompilation(@"
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
");

            RunGenerator(inputCompilation, out var outputCompilation, out var diagnostics);

            diagnostics.Should().BeEmpty();

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

            outputCompilation.SyntaxTrees
                .Should().HaveCount(2)
                .And.Subject.Last()
                    .Should().Be(expected);
        }

        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

        private GeneratorDriver RunGenerator(Compilation inputCompilation, out Compilation outputCompilation,
            out ImmutableArray<Diagnostic> diagnostics)
        {
            var generator = new SourceGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            return driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out outputCompilation, out diagnostics);
        }
    }

    public static class SyntaxTreeExtensions
    {
        public static SyntaxTreeAssertions Should(this SyntaxTree instance)
        {
            return new SyntaxTreeAssertions(instance);
        }

        public class SyntaxTreeAssertions : ReferenceTypeAssertions<SyntaxTree, SyntaxTreeAssertions>
        {
            public SyntaxTreeAssertions(SyntaxTree instance)
            {
                Subject = instance;
            }

            protected override string Identifier => "syntaxTree";

            public AndConstraint<SyntaxTreeAssertions> Be(SyntaxTree syntaxTree)
            {
                var diff = Subject.GetChanges(syntaxTree);
                if (diff.Any())
                {
                    throw new SyntaxTreesNotEqualException(Subject, syntaxTree);
                }


                return new AndConstraint<SyntaxTreeAssertions>(this);
            }
        }

        public class SyntaxTreesNotEqualException : AssertActualExpectedException
        {
            private const string _message = "Generated SyntaxTree differs from the expected one.";

            public SyntaxTreesNotEqualException(
                SyntaxTree expected,
                SyntaxTree actual)
                : base(expected, actual, _message, "Expected SyntaxTree", "Actual SyntaxTree") { }
        }
    }
}
