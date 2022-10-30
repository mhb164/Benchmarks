using BenchmarkDotNet.Running;
using Benchmarks;


var benchmarkTypes = typeof(BenchmarkBase).Assembly.GetTypes()
    .Where(x => x.BaseType == typeof(BenchmarkBase))
    .ToList();

while (true)
{
    for (int i = 0; i < benchmarkTypes.Count; i++)
    {
        Console.WriteLine($"{i + 1}> {benchmarkTypes[i].Name}");
    }
    Console.WriteLine("Enter benchmark number or zero for exit:");
    var numberText = Console.ReadLine();

    if (!int.TryParse(numberText, out var number))
    {
        continue;
    }

    if (number == 0)
    {
        Environment.Exit(0);
    }

    if (number > benchmarkTypes.Count)
    {
        continue;
    }

    var benchmarkType = benchmarkTypes[number-1];
    Console.WriteLine($"running '{benchmarkType}' ...");
    Console.WriteLine(BenchmarkRunner.Run(benchmarkType));
}