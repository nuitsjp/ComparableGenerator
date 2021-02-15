# ComparableGenerator

C# Source Generator to implement IComparable.

NuGet : [ComparableGenerator](https://www.nuget.org/packages/ComparableGenerator/)

```cmd
Install-Package ComparableGenerator
```

## Introduction

For example, if you want to sort the Employee class by the Id property, declare the Comparable and CompareBy attributes.

```cs
using ComparableGenerator;

namespace GenerateSource
{
    [Comparable]
    public partial struct Employee
    {
        [CompareBy]
        public int Id { get; set; }
    }
}```

The ComparableGenerator generates the following code.

```cs
    public partial class Employee : IComparable, IComparable<Employee>
    {
#nullable disable
        public int CompareTo(object other)
#nullable enable
        {
            if (other is null) return 1;

            if (other is Employee concreteObject)
            {
                return CompareTo(concreteObject);
            }

            throw new ArgumentException("Object is not a GenerateSource.Employee.");
        }

#nullable disable
        public int CompareTo(Employee other)
#nullable enable
        {
            if (other is null) return 1;

            static int LocalCompareTo<T>(T? left, T? right) where T : IComparable
            {
                if (left is null && right is null) return 0;

                if (left is null) return -1;

                if (right is null) return 1;

                return left.CompareTo(right);
            }

            return LocalCompareTo(Id, other.Id);
        }
    }

```

## Constraints
- Internal classes are not supported.
- Comparable and CompareBy contained in different "partial sources" are not supported.
- Not analyzing whether the declared member of CompareBy implements IComparable.

