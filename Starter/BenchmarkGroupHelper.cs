namespace Benchmarks;

public static class BenchmarkGroupHelper
{
    public static Dictionary<int, (Type Type, int Number)> GetBenchmarks()
        => typeof(BenchmarkGroupHelper).Assembly.GetTypes()
        .Select((Type, a) => ((Attribute.GetCustomAttribute(Type, typeof(BenchmarkGroupAttribute)) as BenchmarkGroupAttribute), Type))
        .Where(x => x.Item1 is not null)
        .Select((x, number) => (x.Type, x.Item1.Number))
        .ToDictionary(x => x.Number);
}
