using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Jobs;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Benchmarks;

[MemoryDiagnoser(true), BenchmarkGroup(8)]
//[SimpleJob(runtimeMoniker: RuntimeMoniker.Net60)]
//[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70)]
[Config(typeof(Config))]
public class AdventOfCodeDay06AtomicBenchmark
{
    private class Config : ManualConfig
    {
        public Config()
        {
            HideColumns("Job", "Error", "StdDev", "RatioSD");
        }
    }

    [Params("abcdefghijk", "abcdee")]
    public string DataStream { get; set; }

    [Benchmark(Description = "Atomic#1 HasDuplicate#1")]
    public bool HasDuplicate1()
    {
        return DataStream.Distinct().Count() != DataStream.Length;
    }

    [Benchmark(Description = "Atomic#1 HasDuplicate#2")]
    public bool HasDuplicate2_1()
    {
        var dataStreamAsSpan = DataStream.AsSpan();
        for (int i = 0; i < dataStreamAsSpan.Length - 1; i++)
            for (int j = i + 1; j < dataStreamAsSpan.Length; j++)
                if (dataStreamAsSpan[i] == dataStreamAsSpan[j])
                    return true;
        return false;
    }

    [Benchmark(Description = "Atomic#1 HasDuplicate#3")]
    public bool HasDuplicate3()
    {
        var dataStreamAsSpan = DataStream.AsSpan();
        var set = new HashSet<char>(dataStreamAsSpan.Length);
        foreach (var item in dataStreamAsSpan)
            if (!set.Add(item))
                return true;
        return false;
    }
}

