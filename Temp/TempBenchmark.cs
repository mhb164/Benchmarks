using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Jobs;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Benchmarks;

[MemoryDiagnoser(true), BenchmarkGroup(1001)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net60)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70)]
[Config(typeof(Config))]
public class TempBenchmark
{
    private class Config : ManualConfig
    {
        public Config()
        {
            
        }
    }

    //[Params(1000, 100_000)]
    //public const int Loop = 10_000;

    //private Rectangle[] _items;
    private Rectangle rectangle;

    [GlobalSetup]
    public void GlobalSetup()
    {
        //_items = new Rectangle[ItemCount];
        //for (int i = 0; i < ItemCount; i++)
        //    _items[i] = new Rectangle($"Hatch{i}", 5 * (i + 1), 7 * (i + 1));
        rectangle = new Rectangle($"Hatch", 5, 7);
    }

    public int OperateNormal(Rectangle r) => r.Width * r.Height;

    public int OperateByIn(in Rectangle r) => r.Width * r.Height;

    [Benchmark]
    public void Normal()
    {
        //for (int i = 0; i < Loop; i++) OperateNormal(rectangle);
        OperateNormal(rectangle);
    }


    [Benchmark]
    public void ByIn()
    {
        //for (int i = 0; i < Loop; i++) OperateByIn(in rectangle);
        OperateByIn(in rectangle);
    }

    public readonly struct Rectangle
    {
        public Rectangle(string name, int width, int height)
        {
            Id = Guid.NewGuid();
            Name = name;
            Extra1 = string.Join("", Enumerable.Range(0, new Random().Next(200, 500)));
            Extra2 = string.Join("", Enumerable.Range(0, new Random().Next(2000, 5000)));
            Extra3 = string.Join("", Enumerable.Range(0, new Random().Next(200_000, 500_000)));
            Width = width;
            Height = height;
        }

        public readonly Guid Id { get;  }
        public readonly string Name { get;  }
        public readonly string Extra1 { get;}
        public readonly string Extra2 { get; }
        public readonly string Extra3 { get; }
        public readonly int Width { get;  }
        public readonly int Height { get;  }

    }
}

