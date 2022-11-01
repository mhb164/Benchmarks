using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks;

[AttributeUsage(AttributeTargets.Class)]
public class BenchmarkGroupAttribute : Attribute
{
    public BenchmarkGroupAttribute(int number)
    {
        Number = number;
    }

    public int Number { get; private set; }
}
