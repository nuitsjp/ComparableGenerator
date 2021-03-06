# ComparableGenerator

[Japanese](README-ja.md)

C# Source Generator to implement IComparable.

Implementing IComparable correctly is somewhat tedious. It is even more tedious to test each time.

ComparableGenerator provides a high quality implementation of IComparable.

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
}
```

The ComparableGenerator generates the following code.

```cs
using System;

namespace GenerateSource
{
    public partial struct Employee : IComparable, IComparable<Employee>
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

        public int CompareTo(Employee other)
        {
            return Id.CompareTo(other.Id);
        }
    }
}
```

Of course, it also supports classes and multiple members.

```cs
[Comparable]
public partial class ClassObject
{
    [CompareBy]
    public int Value1 { get; set; }

    [CompareBy(Priority = 2)]
    public int Value2;

    [CompareBy(Priority = 1)]
    public int Value3 { get; set; }
}
```

When declaring CompareBy for multiple members, specify a unique priority for each.

[Here](https://github.com/nuitsjp/ComparableGenerator/blob/main/Source/Sample/ClassObject.Partial.cs) is the code generated in this case.


## Environments

- Supports C# 9.0 and higher

## Constraints

- Inner classes are not supported.
- Comparable and CompareBy contained in different "partial sources" are not supported.

## License

This library is under the MIT License.

