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

[MemoryDiagnoser(true), BenchmarkGroup(9)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net60)]
//[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70)]
[Config(typeof(Config))]
public class EnumerateBenchmark
{
    private class Config : ManualConfig
    {
        public Config()
        {
            HideColumns("Job", "Error", "Median", "StdDev", "RatioSD");
        } 
    }

    [Params(100_000)]
    public int LoopCount { get; set; }

    [Benchmark]
    public int For()
    {
        var Guids = new List<Guid>();
        for(int i = 1; i < LoopCount; i++)
        {
            Guids.Add(Guid.NewGuid());
        }
        return Guids.Count;
    }

    [Benchmark]
    public int ForInitialCapacity()
    {
        var Guids = new List<Guid>(LoopCount);
        for (int i = 1; i < LoopCount; i++)
        {
            Guids.Add(Guid.NewGuid());
        }
        return Guids.Count;
    }

    [Benchmark]
    public int EnumrableRange()
    {                    
        var Guids = 
            Enumerable.Range(1, LoopCount)
                      .Select(x=> Guid.NewGuid());
        return Guids.Count();
    }

    [Benchmark]
    public int EnumrableRangeToList()
    {
        var Guids = 
            Enumerable.Range(1, LoopCount)
                      .Select(x => Guid.NewGuid())
                      .ToList();
        return Guids.Count;
    }
}

