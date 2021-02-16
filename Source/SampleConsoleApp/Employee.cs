using ComparableGenerator;

namespace SampleConsoleApp
{
    [Comparable]
    public partial class Employee
    {
        [CompareBy(Priority = 2)]
        public string FirstName { get; set; }

        [CompareBy(Priority = 1)]
        public string LastName { get; set; }
    }
}