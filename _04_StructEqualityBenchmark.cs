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


//https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record#value-equality
/*
 The definition of equality for a record struct is the same as for a struct. 
 The difference is that for a struct, the implementation is in ValueType.Equals(Object) and relies on reflection. 
 For records, the implementation is compiler synthesized and uses the declared data members.
 */

[MemoryDiagnoser, BenchmarkGroup(4)]
[Config(typeof(Config))]
//[SimpleJob(runtimeMoniker: RuntimeMoniker.Net60, baseline: true)]
//[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70)]
//[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
public class StructEqualityBenchmark
{
    private class Config : ManualConfig
    {
        public Config()
        {
            HideColumns("Job", "Error", "StdDev", "RatioSD");
        }
    }

    public struct PersonStruct
    {
        public string Firstname { get; init; }
        public string Lastname { get; init; }
    }

    public record struct PersonRecordStruct(string Firstname, string Lastname);

    public struct PersonStructIEquatable : IEquatable<PersonStructIEquatable>
    {
        public string Firstname { get; init; }

        public string Lastname { get; init; }

        public override bool Equals(object? obj)
            => obj is PersonStructIEquatable other && Equals(other);

        public bool Equals(PersonStructIEquatable other)
            => Firstname == other.Firstname && Lastname == other.Lastname;

        public static bool operator ==(PersonStructIEquatable left, PersonStructIEquatable right) => left.Equals(right);

        public static bool operator !=(PersonStructIEquatable left, PersonStructIEquatable right) => !(left == right);

        public override int GetHashCode() => (Firstname, Lastname).GetHashCode();
    }


    private const string _firstname = "John";
    private const string _lastname = "Wick";

    private PersonStruct PersonStruct1;
    private PersonStruct PersonStruct2;

    private PersonStructIEquatable PersonStructIEquatable1;
    private PersonStructIEquatable PersonStructIEquatable2;

    private PersonRecordStruct PersonRecordStruct1;
    private PersonRecordStruct PersonRecordStruct2;

    [GlobalSetup]
    public void GlobalSetup()
    {
        PersonStruct1 = new PersonStruct() { Firstname = _firstname, Lastname = _lastname };
        PersonStruct2 = new PersonStruct() { Firstname = _firstname, Lastname = _lastname };

        PersonStructIEquatable1 = new PersonStructIEquatable() { Firstname = _firstname, Lastname = _lastname };
        PersonStructIEquatable2 = new PersonStructIEquatable() { Firstname = _firstname, Lastname = _lastname };

        PersonRecordStruct1 = new PersonRecordStruct(_firstname, _lastname);
        PersonRecordStruct2 = new PersonRecordStruct(_firstname, _lastname);
    }


    [Benchmark]
    public bool Struct() => PersonStruct1.Equals(PersonStruct2);

    [Benchmark]
    public bool RecordStruct() => PersonRecordStruct1.Equals(PersonRecordStruct2);

    [Benchmark]
    public bool IEquatableImplementedStruct() => PersonStructIEquatable1.Equals(PersonStructIEquatable2);

}

