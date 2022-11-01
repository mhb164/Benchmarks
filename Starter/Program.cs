using BenchmarkDotNet.Running;
using Benchmarks;

var benchmarkTypes = typeof(BenchmarkBase).Assembly.GetTypes()
    .Where(x => x.BaseType == typeof(BenchmarkBase))
    .ToList();

while (true)
{
    Console.WriteLine("--------------< Benchmarks >--------------");
    Console.WriteLine($" [0] Exit");
    for (int i = 0; i < benchmarkTypes.Count; i++)
    {
        Console.WriteLine($" [{i + 1}] {benchmarkTypes[i].Name}");
    }
    Console.Write("Enter number: ");
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

    var benchmarkType = benchmarkTypes[number - 1];
    Console.WriteLine($"Running '{benchmarkType}' ...");
    Console.WriteLine(BenchmarkRunner.Run(benchmarkType));
}