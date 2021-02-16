using System;
using System.Collections.Generic;

namespace SampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var employees = new List<Employee>();
            employees.Add(new Employee {LastName = "Tanaka", FirstName = "Taro"});
            employees.Add(new Employee {LastName = "Suzuki", FirstName = "Jiroh"});
            employees.Add(new Employee {LastName = "Suzuki", FirstName = "Ichiroh"});

            employees.Sort();

            employees.ForEach(x => Console.WriteLine($"{x.LastName} {x.FirstName}"));
        }
    }
}
