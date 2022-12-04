using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Jobs;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using System;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Benchmarks;

[MemoryDiagnoser(true), BenchmarkGroup(6)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net60)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70)]
[Config(typeof(Config))]
public class InterpolatedStringBenchmark
{
    private class Config : ManualConfig
    {
        public Config()
        {
            HideColumns("Job", "Error", "StdDev", "RatioSD");
        }
    }


    public int _int;
    public double _double;
    public string _Guid;
    public string _string;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _int = int.MaxValue;
        _double = double.MaxValue;
        _Guid = Guid.NewGuid().ToString();
        _string = "Listen to the reed and the tale it tells, How it sings of separation";
    }

    [Benchmark]
    public void PassDirectToStringFormat()
    {
        var i = 0;
        while (i++ < 100)
            _ = string.Format("int: {0}, double: {1}, Guid: {2}, string: {3}", _int, _double, _Guid, _string);
    }


    [Benchmark(Baseline = true)]
    public void InterpolatedString()
    {
        var i = 0;
        while (i++ < 100)
            _ = $"int: {_int}, double: {_double}, Guid: {_Guid}, string: {_string}";
    }

    [Benchmark]
    public void UsePlusInLine()
    {
        var i = 0;
        while (i++ < 100)
            _ = "int: " + _int + ", double: " + _double + ", Guid: " + _Guid + ", string: " + _string;

    }

}

