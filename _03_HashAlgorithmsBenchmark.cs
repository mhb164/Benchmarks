using BenchmarkDotNet.Attributes;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Benchmarks;

[MemoryDiagnoser, BenchmarkGroup(3)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net60, baseline: true)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70)]
[Config(typeof(Config))]
public class HashAlgorithmsBenchmark
{

    private class Config : ManualConfig
    {
        public Config()
        {
            HideColumns("Job", "Error", "StdDev", "RatioSD");
        }
    }

    public string Password;
    public byte[] PasswordAsBytes;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Password = "MyP@ssw0rd!";
        PasswordAsBytes = Encoding.UTF8.GetBytes(Password);
    }

    [Benchmark(Description = "MD5")]
    public void MD5Hash()
    {
        using var hashAlgorithm = MD5.Create();
        _ = hashAlgorithm.ComputeHash(PasswordAsBytes);
    }

    [Benchmark(Description = "SHA1")]
    public void SHA1Hash()
    {
        using var hashAlgorithm = SHA1.Create();
        _ = hashAlgorithm.ComputeHash(PasswordAsBytes);
    }

    [Benchmark(Description = "SHA256")]
    public void HashBySHA256()
    {
        using var hashAlgorithm = SHA256.Create();
        _ = hashAlgorithm.ComputeHash(PasswordAsBytes);
    }

    [Benchmark(Description = "SHA384")]
    public void HashBySHA384()
    {
        using var hashAlgorithm = SHA384.Create();
        _ = hashAlgorithm.ComputeHash(PasswordAsBytes);
    }

    [Benchmark(Description = "SHA512")]
    public void HashSHA512()
    {
        using var hashAlgorithm = SHA512.Create();
        _ = hashAlgorithm.ComputeHash(PasswordAsBytes);
    }

}
