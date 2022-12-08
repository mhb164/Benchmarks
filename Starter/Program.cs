using BenchmarkDotNet.Running;
using Benchmarks;
using Benchmarks.ProofOfConcepts;
using System.Reflection;
using System.Text;

internal class Program
{
    public static string ProductVersion => Assembly.GetAssembly(typeof(Program)).GetName().Version.ToString();
    private static void Main(string[] args)
    {
        //new EnumerateProofOfConcept().Run(args); return;

        //var benchmark = new TempBenchmark();
        //benchmark.GlobalSetup();
        ////benchmark.NormalLoad();
        //benchmark.MetadataLoadContext();

        //BenchmarkSwitcher.FromAssembly(typeof(ForeachWithAnIndexBenchmark).Assembly).Run();

        var benchmarks = BenchmarkGroupHelper.GetBenchmarks();

        if (args.Any())
        {
            if (HelpRequired(args.FirstOrDefault()))
            {
                Console.WriteLine(GenerateHelp(benchmarks));
                return;
            }

            if (int.TryParse(args.First(), out var selectedNumber))
            {
                if (!benchmarks.ContainsKey(selectedNumber))
                {
                    ShowMenu(benchmarks, $"'{selectedNumber}' number is wrong!");
                    return;
                }
                else
                {
                    RunBenchmark(selectedNumber, benchmarks[selectedNumber].Type);
                    return;
                }
            }
        }

        ShowMenu(benchmarks);
    }

    private static bool HelpRequired(string param)
    {
        return param == "-h" || param == "--help" || param == "/?";
    }

    private static string GenerateHelp(Dictionary<int, (Type Type, int Number)> benchmarks)
    {
        var help = new StringBuilder();
        help.AppendLine($" Benchmarks v{ProductVersion} ");
        foreach (var benchmark in benchmarks.Values.OrderBy(x => x.Number))
        {
            help.AppendLine($" [{benchmark.Number}] {benchmark.Type.Name}");
        }
        return help.ToString();
    }

    private static void ShowMenu(Dictionary<int, (Type Type, int Number)> benchmarks, string message = null)
    {
        Console.WriteLine($" ----------------");
        if (!string.IsNullOrEmpty(message))
        {
            Console.WriteLine(message);
        }

        Console.WriteLine($" Benchmarks v{ProductVersion} ");
        Console.WriteLine($" [0] Exit");
        foreach (var benchmark in benchmarks.Values.OrderBy(x => x.Number))
        {
            Console.WriteLine($" [{benchmark.Number}] {benchmark.Type.Name}");
        }
        Console.Write("Enter number: ");
        var userChoise = Console.ReadLine();

        if (!int.TryParse(userChoise, out var selectedNumber))
        {
            ShowMenu(benchmarks, $"'{userChoise}' is not a number!");
            return;
        }

        if (selectedNumber == 0)
        {
            Environment.Exit(0);
        }

        if (!benchmarks.ContainsKey(selectedNumber))
        {
            ShowMenu(benchmarks, $"'{selectedNumber}' number is wrong!");
            return;
        }

        RunBenchmark(selectedNumber, benchmarks[selectedNumber].Type);
    }

    private static void RunBenchmark(int number, Type benchmarkType)
    {
        Console.WriteLine($"Running '[{number}] {benchmarkType.Name}' ...");
        Console.WriteLine(BenchmarkRunner.Run(benchmarkType));
    }
}