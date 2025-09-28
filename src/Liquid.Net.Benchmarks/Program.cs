using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using Liquid.Net.Benchmarks;
using Liquid.Net.Benchmarks.Comparative;
using Liquid.Net.Benchmarks.Data;

namespace Liquid.Net.Benchmarks;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Liquid.Net Benchmark Suite");
        Console.WriteLine("==========================");
        Console.WriteLine("Comprehensive evaluation against original LNN test corpus");
        Console.WriteLine();

        if (args.Length > 0 && args[0].ToLower() == "micro")
        {
            // Run micro-benchmarks using BenchmarkDotNet
            Console.WriteLine("Running micro-benchmarks with BenchmarkDotNet...");
            BenchmarkRunner.Run<LiquidNetMicroBenchmarks>();
        }
        else if (args.Length > 0 && args[0].ToLower() == "datasets")
        {
            // Show available datasets
            ShowAvailableDatasets();
        }
        else if (args.Length > 0 && args[0].ToLower() == "comparative")
        {
            // Run comparative benchmarks against baseline algorithms
            Console.WriteLine("Running comparative evaluation against baseline algorithms...");
            Console.WriteLine("This replicates the methodology from original LNN research papers:");
            Console.WriteLine("- Hasani et al. 'Liquid Time-constant Networks' (2020)");
            Console.WriteLine("- Lechner et al. 'Neural Circuit Policies' (2020)");
            Console.WriteLine();

            var comparativeSuite = new ComparativeBenchmarkSuite();
            await comparativeSuite.RunComparativeEvaluation();
        }
        else if (args.Length > 0 && args[0].ToLower() == "all")
        {
            // Run all benchmark suites
            await RunAllBenchmarks();
        }
        else
        {
            // Run comprehensive benchmarks against standard LNN test corpus
            await RunStandardBenchmarks();
        }
    }

    private static void ShowAvailableDatasets()
    {
        Console.WriteLine("Available benchmark datasets from original LNN research:");
        Console.WriteLine();

        foreach (var dataset in StandardDatasets.GetAllDatasets())
        {
            Console.WriteLine($"Dataset: {dataset.Name}");
            Console.WriteLine($"  Description: {dataset.Description}");
            Console.WriteLine($"  Input dimensions: {dataset.InputDimension}");
            Console.WriteLine($"  Output dimensions: {dataset.OutputDimension}");
            Console.WriteLine($"  Sequence length: {dataset.SequenceLength}");
            Console.WriteLine($"  Samples: {dataset.Inputs.GetLength(0)}");
            Console.WriteLine($"  Source: {dataset.Source}");
            Console.WriteLine();
        }

        Console.WriteLine("These datasets represent the standard test corpus used to evaluate");
        Console.WriteLine("Liquid Neural Networks in academic research and publications.");
    }

    private static async Task RunStandardBenchmarks()
    {
        Console.WriteLine("Running comprehensive benchmarks against original LNN test corpus...");
        Console.WriteLine();
        Console.WriteLine("Standard test corpus includes:");
        Console.WriteLine("- Mackey-Glass chaotic time series prediction");
        Console.WriteLine("- Noisy sine wave continuous learning tasks");
        Console.WriteLine("- Lorenz attractor dynamical system modeling");
        Console.WriteLine();
        Console.WriteLine("These benchmarks evaluate core LNN capabilities:");
        Console.WriteLine("✓ Temporal pattern recognition");
        Console.WriteLine("✓ Chaotic system modeling");
        Console.WriteLine("✓ Continuous learning and adaptation");
        Console.WriteLine("✓ Memory efficiency and computational performance");
        Console.WriteLine();

        var runner = new LiquidNetBenchmarkRunner();
        var results = await runner.RunComprehensiveBenchmarks();

        Console.WriteLine("Standard benchmarking completed!");
        DisplayUsageOptions();

        Console.WriteLine("Results Summary:");
        Console.WriteLine($"- Total benchmark runs: {results.Count}");
        Console.WriteLine($"- Average MSE across all tests: {results.Average(r => r.MeanSquaredError):F6}");
        Console.WriteLine($"- Best performing model: {results.OrderBy(r => r.MeanSquaredError).First().ModelName}");
        Console.WriteLine($"- Total datasets evaluated: {results.Select(r => r.DatasetName).Distinct().Count()}");
        Console.WriteLine();
        Console.WriteLine("These benchmarks evaluate the Liquid.Net framework against the same");
        Console.WriteLine("test corpus used in the original MIT LNN research papers, ensuring");
        Console.WriteLine("direct performance comparison with published academic results.");
    }

    private static async Task RunAllBenchmarks()
    {
        Console.WriteLine("Running complete benchmark suite...");
        Console.WriteLine("This includes both standard corpus evaluation and comparative analysis.");
        Console.WriteLine();

        // Run standard benchmarks
        var runner = new LiquidNetBenchmarkRunner();
        var standardResults = await runner.RunComprehensiveBenchmarks();

        Console.WriteLine();
        Console.WriteLine("=" + new string('=', 60));
        Console.WriteLine();

        // Run comparative benchmarks
        var comparativeSuite = new ComparativeBenchmarkSuite();
        var comparativeResults = await comparativeSuite.RunComparativeEvaluation();

        Console.WriteLine();
        Console.WriteLine("=== COMPLETE BENCHMARK SUITE SUMMARY ===");
        Console.WriteLine($"Standard benchmarks completed: {standardResults.Count} tests");
        Console.WriteLine($"Comparative benchmarks completed: {comparativeResults.Count} comparisons");
        Console.WriteLine($"Total datasets evaluated: {standardResults.Select(r => r.DatasetName).Distinct().Count()}");
        Console.WriteLine($"Baseline algorithms compared: {comparativeResults.Select(r => r.Algorithm).Distinct().Count()}");
        Console.WriteLine();
        Console.WriteLine("This comprehensive evaluation covers:");
        Console.WriteLine("✓ Performance against original LNN test corpus");
        Console.WriteLine("✓ Comparative analysis vs traditional neural networks");
        Console.WriteLine("✓ Memory efficiency and computational benchmarks");
        Console.WriteLine("✓ Adaptability and continuous learning metrics");
    }

    private static void DisplayUsageOptions()
    {
        Console.WriteLine();
        Console.WriteLine("Usage options:");
        Console.WriteLine("  dotnet run                    - Run standard LNN corpus benchmarks");
        Console.WriteLine("  dotnet run comparative        - Run comparative analysis vs baselines");
        Console.WriteLine("  dotnet run micro             - Run micro-benchmarks with BenchmarkDotNet");
        Console.WriteLine("  dotnet run datasets          - Show available benchmark datasets");
        Console.WriteLine("  dotnet run all               - Run complete benchmark suite");
        Console.WriteLine();
    }
}
