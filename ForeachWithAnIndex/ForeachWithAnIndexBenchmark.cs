using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeachWithAnIndex;

[MemoryDiagnoser]
public class ForeachWithAnIndexBenchmark
{
    [Params(100, 100_000)]
    public int ItemCount { get; set; }

    private static List<string> _items;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _items = Enumerable.Range(0, ItemCount).Select(x => x.ToString()).ToList();
    }

    [Benchmark]
    public void ByVariable()
    {
        var index = 0;
        foreach (var greekLetter in _items)
        {
            DoSomeThing(index, greekLetter);
            index++;
        }
    }

    [Benchmark]
    public void BySelect()
    {
        foreach (var item in _items.Select((value, i) => new { i, value }))
        {
            DoSomeThing(item.i, item.value);
        }
    }

    private static void DoSomeThing(int index, string letter) => _ = index;
}
