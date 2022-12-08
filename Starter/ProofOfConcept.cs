using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks;

public abstract class ProofOfConcept
{
    public void Run(params string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"------------ ProofOfConcept {GetType().Name.Replace("ProofOfConcept", "")} ------------");
        Console.ForegroundColor = ConsoleColor.Green;
        run(args);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public abstract int Number { get; }

    public abstract void run(string[] args);
}