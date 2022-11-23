using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Iced.Intel;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;

namespace Benchmarks;


[MemoryDiagnoser(true), BenchmarkGroup(5)]
[Config(typeof(Config))]
public partial class ExceptionBenchmark
{
    private class Config : ManualConfig
    {
        public Config()
        {
            HideColumns("Job", "Error", "StdDev");
        }
    }
}

public partial class ExceptionBenchmark
{
    private const string _notDigit = "5A";

    [Benchmark]
    public string ThrowException()
    {
        try
        {
            return $"{int.Parse(_notDigit)} is digit";
        }
        catch
        {
            return $"{_notDigit} isn't digit";
        }
    }


    [Benchmark]
    public string AvoidException()
    {
        if (int.TryParse(_notDigit, out var digit))
        {
            return $"{digit} is digit";
        }
        else
        {
            return $"{_notDigit} isn't digit";
        }
    }

}

