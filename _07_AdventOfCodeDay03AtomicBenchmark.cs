using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Jobs;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Benchmarks;

[MemoryDiagnoser(true), BenchmarkGroup(7)]
//[SimpleJob(runtimeMoniker: RuntimeMoniker.Net60)]
//[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70)]
[Config(typeof(Config))]
public class AdventOfCodeDay03AtomicsBenchmark
{
    private class Config : ManualConfig
    {
        public Config()
        {
            HideColumns("Job", "Error", "StdDev", "RatioSD");
        }
    }

    public string[] Lines;
    public List<string[]> HalvedLines;
    public char[] PartOneBadges;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Lines = new string[]
        {
            "GGVGlqWFgVfFqqVZGFlblJPMsDbbMrDMpDsJRn",
            "LwzHtwdLHHwDrzPZzzsJbJ",
            "wdLTBvSvHvZVGCjhfN",
            "HsSSnZVHjjssZnJpSJjBHHWgQGcgqqQLQdQFqNgWgqGNDg",
            "rmmRwrtfThtTrbCrGGGcLBDTqDBNQLdL",
            "mwPrrbzPfwvbzhwMMnnjHnBjZlnzMM",
            "gjjdMBgdqdTpJpBcjgRRRlqnvrqfnZtrtZDw",
            "zHShWLhCszCWHVbVzQWCPtQvNZRwtfftfNnrnftlfR",
            "PzPSssHbVhCLFMJFcMFJJMjdJw",
            "ZqJdtpfpjmpJjpnwWdttTCDLLTQFNTzTzrcqrQqc",
            "MsSlBGvBsSGGSlbGsCgggNTgzNLczFQNrNQVQcFzFF",
            "sGHHSGllhvMGhSRGCjtjtjnjnnmHWpWWtJ",
            "tMdjQlHPHsGjsCtsCpwwqfhfnnFMDMrpfD",
            "SbNvWvBRJRWwFSgppgSrfg",
            "RNcNbvzJRcVLRVzTRFLjdHCQttdZdPlHstPl",
            "QWqgpdBflpHNCNWNHHPm",
            "VVMbbJsLFVMhrMJMmRjFNHwHjjCTGSSRFj",
            "mbMsZzsLmVhJZrcLcJhLMtnqvBnZdggplDffvlnlvnDn",
            "prnNnsFnZpnBNdNtFrNnzNQQwTTQZqTHTQJQMwHDMDlZ",
            "jgfgcSmbLmhmcPShghRdmwJTQjTlqGlJQJHqQqGHqQ",
            "hRVhPfbCgbVggLVRSSmRhRPhrrrnCzzsvCvrnvFnNppsvBtd",
            "QJLNDWSWQdLFFFhLdt",
            "npHhHMsfsjpZjznRtmrMCdBwFBFrBdmt",
            "HsjHqRRfnnHRsgfHffZspgzqDGQSWbQTDNGhQhSqNPhDWWbT",
            "bsCmFDsGZCNsDmLDLZBSHSJTHnrZQMQSSQ",
            "jqRpwvfqnnRQrftdBMHddB",
            "phpchwpzjpvwRzwcsnlFsssPCCGzDlsD",
            "rMqzVQfrfVZWZhTdRTQL",
            "cgmtFtjFFJDDtFvSFRZdLlhpHZddmwTZWh",
            "FbcSTtctcvFTJNgtJDGNPnCqMPMfMBfznGVsrMCq",
        };

        HalvedLines = Lines.Select(l => new string[] { l[..(l.Length / 2)], l[(l.Length / 2)..] })
                           .ToList();

        PartOneBadges = HalvedLines.Select(x => x[0].Intersect(x[1]).First()).ToArray();
    }

    [Benchmark(Description = "Atomic#1 AsSpan")]
    public void Atomic1AsSpan()
    {
        foreach (var line in Lines)
        {
            var lineAsSpan = line.AsSpan();
            _ = lineAsSpan[..(lineAsSpan.Length / 2)];
        }
    }

    [Benchmark(Description = "Atomic#1 Normal split")]
    public void Atomic1Substring()
    {
        foreach (var line in Lines)
        {
            _ = line[..(line.Length / 2)];
        }
    }

    [Benchmark(Description = "Atomic#2 Simple Loop Halves")]
    public void Atomic2SimpleLoop()
    {
        foreach (var halvedLine in HalvedLines)
        {
            var badge = '?';
            foreach (var item in halvedLine[0])
                if (halvedLine[1].Contains(item))
                {
                    badge = item;
                    break;
                }
            _ = badge;
        }
    }

    [Benchmark(Description = "Atomic#2 Intersect Halves")]
    public void Atomic2Intersect()
    {
        foreach (var halvedLine in HalvedLines)
        {
            var badge = halvedLine[0].Intersect(halvedLine[1]).First();
            _ = badge;
        }
    }

    [Benchmark(Description = "Atomic#3 Simple Loop Three")]
    public void Atomic3SimpleLoop()
    {
        for (int i = 0; i < Lines.Length / 3; i++)
        {
            var first = Lines[i * 3 + 0];
            var second = Lines[i * 3 + 1];
            var third = Lines[i * 3 + 2];

            var badge = ' ';
            foreach (var item in first)
                if (second.Contains(item) && third.Contains(item))
                {
                    badge = item;
                    break;
                }

            _ = badge;
        }
    }

    [Benchmark(Description = "Atomic#3 Intersect Three")]
    public void Atomic3Intersect()
    {
        for (int i = 0; i < Lines.Length / 3; i++)
        {
            var first = Lines[i * 3 + 0];
            var second = Lines[i * 3 + 1];
            var third = Lines[i * 3 + 2];

            var badge = first.Intersect(second.Intersect(third)).First();
            _ = badge;
        }
    }

    [Benchmark(Description = "Atomic#4 Periority by Grather")]
    public void Atomic4PeriorityGrather()
    {
        var sum = 0;
        foreach (var badge in PartOneBadges)
        {
            if (badge > 97) sum += (badge - 'a') + 1;
            else sum += (badge - 'A') + 27;
        }
    }

    [Benchmark(Description = "Atomic#4 Periority by IsLower")]
    public void Atomic4PeriorityIsLower() 
    {
        var sum = 0;
        foreach (var badge in PartOneBadges)
        {
            if (char.IsLower(badge))
            {
                sum += (badge - 'a') + 1;
            }
            else
            {
                sum += (badge - 'A') + 27;
            }
        }
    }
}

