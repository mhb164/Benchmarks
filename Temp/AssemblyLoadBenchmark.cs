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
using BenchmarkDotNet.Characteristics;
using Microsoft.Diagnostics.Runtime.DacInterface;
using System.Runtime.InteropServices;
using static Benchmarks.InstanceCreationBenchmark;

namespace Benchmarks;

[MemoryDiagnoser, BenchmarkGroup(1002)]
public class AssemblyLoadBenchmark
{
    private const string AssemblyPath = @"Z:\BenchmarkDotNet.dll";
    private PathAssemblyResolver resolver;
    private MetadataLoadContext metadataLoadContext;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var assemblyPaths = new List<string>(Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll"));
        assemblyPaths.Add(AssemblyPath);
        resolver = new PathAssemblyResolver(assemblyPaths);
        metadataLoadContext = new MetadataLoadContext(resolver);
    }

    [Benchmark]
    public void NormalLoad()
    {
        DoSomeThing(Assembly.LoadFrom(AssemblyPath));
    }

    [Benchmark]
    public void MetadataLoadContextCache()
    {
        DoSomeThing(metadataLoadContext.LoadFromAssemblyPath(AssemblyPath));
    }

    [Benchmark]
    public void MetadataLoadContext()
    {
        using var metadataContext = new MetadataLoadContext(resolver);
        DoSomeThing(metadataContext.LoadFromAssemblyPath(AssemblyPath));
    }

    private void DoSomeThing(Assembly assembly) => _ = assembly.FullName;



}