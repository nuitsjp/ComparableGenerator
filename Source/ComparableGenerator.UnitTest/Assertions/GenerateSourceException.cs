using FluentAssertions.Execution;

namespace ComparableGenerator.UnitTest.Assertions
{
    public class GenerateSourceException : AssertionFailedException
    {
        public GenerateSourceException(string message) : base(message)
        {
        }
    }
}