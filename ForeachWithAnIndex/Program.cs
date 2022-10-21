using ForeachWithAnIndex;
using BenchmarkDotNet.Running;

Console.WriteLine(BenchmarkRunner.Run<ForeachWithAnIndexBenchmark>());