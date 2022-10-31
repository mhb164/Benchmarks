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

namespace Benchmarks;

[MemoryDiagnoser]
public class InstanceCreationBenchmark : BenchmarkBase
{
    private Func<string, Person> CreatePersonByExpression;
    private delegate Person DynamicPersonActivator(string name);
    private DynamicPersonActivator CreatePersonByEmit;

    [GlobalSetup]
    public void GlobalSetup()
    {
        CreatePersonByExpression = GenerateCreatePersonByExpression();
        CreatePersonByEmit = GenerateCreatePersonByEmit();
     
    }

    private Func<string, Person> GenerateCreatePersonByExpression()
    {
        System.Linq.Expressions.Expression<Func<string, Person>> createPersonExpression = name => Activator.CreateInstance(typeof(Person), name) as Person;
        return createPersonExpression.Compile();
    }

    private DynamicPersonActivator GenerateCreatePersonByEmit()
    {
        var dynamicMethod = new DynamicMethod("CreateInstance", typeof(Person), new Type[] { typeof(string) }, true);
        var ilGenerator = dynamicMethod.GetILGenerator();

        ilGenerator.Emit(OpCodes.Nop);
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ConstructorInfo constructor = typeof(Person).GetConstructor(new Type[] { typeof(string) });
        ilGenerator.Emit(OpCodes.Newobj, constructor);
        ilGenerator.Emit(OpCodes.Ret);

        return (DynamicPersonActivator)dynamicMethod.CreateDelegate(typeof(DynamicPersonActivator));
    }

    [Benchmark]
    public void NewOperatorNoConstructor()
    {
        DoSomeThing(new Person());
    }

    [Benchmark]
    public void NewOperatorByConstructor()
    {
        DoSomeThing(new Person("Name"));
    }

    [Benchmark]
    public void DynamicallyNoConstructor()
    {
        DoSomeThing(Activator.CreateInstance(typeof(Person)) as Person);
    }

    [Benchmark]
    public void DynamicallyByConstructor()
    {
        DoSomeThing(Activator.CreateInstance(typeof(Person), "Name") as Person);
    }

    [Benchmark]
    public void DynamicallyByExpressionByConstructor()
    {
        DoSomeThing(CreatePersonByExpression("Name"));
    }

    [Benchmark]
    public void NewOperatorByEmitByConstructor()
    {
        DoSomeThing(CreatePersonByEmit("Name"));
    }

    private void DoSomeThing(Person person) => _ = person.Name;
}

public class Person
{
    public readonly string Name;
    public Person() : this("No Name!") { }
    public Person(string name) { Name = name; }
}
