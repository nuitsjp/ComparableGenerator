namespace ComparableGenerator.UnitTest.Assertions
{
    public class Analyzer
    {
        public string Source { get; }

        public Analyzer(string source)
        {
            Source = source;
        }

        public AnalyzerAssertions Should()
        {
            return new(this);
        }
    }
}