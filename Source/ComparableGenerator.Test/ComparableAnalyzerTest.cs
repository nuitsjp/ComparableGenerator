//using System.Threading.Tasks;
//using Xunit;
//using VerifyCS = ComparableGenerator.Test.Verifiers.CSharpAnalyzerVerifier<
//    ComparableGenerator.ComparableAnalyzer,
//    ComparableGenerator.ComparableCodeFixProvider>;

//namespace ComparableGenerator.Test
//{
//    public class ComparableAnalyzerTest
//    {
//        [Fact]
//        public async Task CompareBy_is_not_defined()
//        {
//            var test = @"
//using ComparableGenerator;

//namespace MyNamespace
//{
//    [Comparable]
//    public class MyClass
//    {
//    }
//}
//";

//            var expected = VerifyCS.Diagnostic("MakeConst").WithArguments("MyClass");
//            await VerifyCS.VerifyAnalyzerAsync(test, expected);

//        }
//    }
//}