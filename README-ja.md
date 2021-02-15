# ComparableGenerator

IComparableを実装するためのC#ソースジェネレーターです。

IComparableを正しく実装するのはやや面倒です。毎回テストするのはさらに面倒です。

ComparableGeneratorはIComparableの高品質な実装を提供します。

NuGet : [ComparableGenerator](https://www.nuget.org/packages/ComparableGenerator/)

```cmd
Install-Package ComparableGenerator
```

## Introduction

たとえば、EmployeeクラスをIdプロパティでソートしたい場合は、Comparable属性とCompareBy属性を宣言します。

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

ComparableGeneratorは以下のコードを生成します。

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

もちろん、クラスや複数のメンバーにも対応しています。

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

複数のメンバーに対してCompareByを宣言する場合は、それぞれにユニークな優先度を指定します。

この場合に生成されるコードは[こちら](https://github.com/nuitsjp/ComparableGenerator/blob/main/Source/Sample/ClassObject.Partial.cs)を参照してください。


## Environments

- C# 9.0以上をサポート

## Constraints

- インナークラスはサポートされていません。
- ComparableとCompareByを異なるpartialファイルに宣言することはサポートされていません。

## ライセンス

このライブラリはMITライセンスに基づいています。
