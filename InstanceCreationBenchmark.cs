using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks;

[MemoryDiagnoser]
public class InstanceCreationBenchmark : BenchmarkBase
{
    [Benchmark]
    public void NewOperatorNoConstructor()
    {
        DoSomeThing(new Person());
    }

    [Benchmark]
    public void NewOperatorByConstructor()
    {
        DoSomeThing(new Person($"Name"));
    }

    [Benchmark]
    public void DynamicallyNoConstructor()
    {
        DoSomeThing(Activator.CreateInstance(typeof(Person)) as Person);
    }

    [Benchmark]
    public void DynamicallyByConstructor()
    {
        DoSomeThing(Activator.CreateInstance(typeof(Person), $"Name") as Person);
    }

    private void DoSomeThing(Person person) => _ = person.Name;
}

public class Person
{
    public readonly string Name;
    public Person() : this("No Name!") { }
    public Person(string name) { Name = name; }
}
