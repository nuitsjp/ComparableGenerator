namespace ComparableGenerator.UnitTest.Assertions
{
    public static class AnalyzerExtensions
    {
        public static Analyzer CreateAnalyzer(this string source) => new (source);
    }
}