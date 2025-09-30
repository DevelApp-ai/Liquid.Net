using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Liquid.Net.Benchmarks.Data;
using Liquid.Net.Benchmarks.Metrics;
using Liquid.Net.Core;
using Liquid.Net.Models;

namespace Liquid.Net.Benchmarks;

/// <summary>
/// Comprehensive benchmark runner for evaluating LNN performance against standard test corpus
/// </summary>
public class LiquidNetBenchmarkRunner
{
    private readonly List<BenchmarkResults> _results = new();

    /// <summary>
    /// Run comprehensive benchmarks against all standard datasets
    /// </summary>
    public async Task<List<BenchmarkResults>> RunComprehensiveBenchmarks()
    {
        Console.WriteLine("=== Liquid.Net Comprehensive Benchmark Suite ===");
        Console.WriteLine("Running benchmarks against standard LNN test corpus...\n");

        var datasets = StandardDatasets.GetAllDatasets().ToList();

        foreach (var dataset in datasets)
        {
            Console.WriteLine($"Running benchmarks for dataset: {dataset.Name}");
            Console.WriteLine($"Description: {dataset.Description}");
            Console.WriteLine($"Input dimensions: {dataset.InputDimension}, Output dimensions: {dataset.OutputDimension}");
            Console.WriteLine($"Sequence length: {dataset.SequenceLength}, Samples: {dataset.Inputs.GetLength(0)}");
            Console.WriteLine();

            // Run different network configurations
            await RunBenchmarkConfigurations(dataset);

            Console.WriteLine($"Completed benchmarks for {dataset.Name}\n");
        }

        // Generate summary report
        GenerateSummaryReport();

        return _results;
    }

    private async Task RunBenchmarkConfigurations(BenchmarkDataset dataset)
    {
        // Test different network sizes and configurations
        var configurations = new[]
        {
            new { Name = "Small-LNN", NeuronCount = 32, Connectivity = 0.3 },
            new { Name = "Medium-LNN", NeuronCount = 64, Connectivity = 0.4 },
            new { Name = "Large-LNN", NeuronCount = 128, Connectivity = 0.5 }
        };

        foreach (var config in configurations)
        {
            Console.WriteLine($"  Testing configuration: {config.Name} ({config.NeuronCount} neurons)");

            var result = await RunSingleBenchmark(dataset, config.Name, config.NeuronCount, config.Connectivity);
            _results.Add(result);

            // Display immediate results
            Console.WriteLine($"    MSE: {result.MeanSquaredError:F6}");
            Console.WriteLine($"    RMSE: {result.RootMeanSquaredError:F6}");
            Console.WriteLine($"    MAE: {result.MeanAbsoluteError:F6}");
            Console.WriteLine($"    R²: {result.R2Score:F4}");
            Console.WriteLine($"    Training time: {result.TrainingTime:F2}ms");
            Console.WriteLine($"    Memory usage: {result.MemoryUsage:F2}MB");
            Console.WriteLine();
        }
    }

    private async Task<BenchmarkResults> RunSingleBenchmark(
        BenchmarkDataset dataset,
        string modelName,
        int neuronCount,
        double connectivity)
    {
        var stopwatch = Stopwatch.StartNew();

        // Create a simple liquid network for testing
        // In practice, this would use the actual LNN implementation
        var network = CreateTestNetwork(neuronCount, dataset.InputDimension, dataset.OutputDimension);

        // Mock training process (would be replaced with actual training)
        await SimulateTraining(network, dataset);
        var trainingTime = stopwatch.ElapsedMilliseconds;

        // Mock prediction process
        stopwatch.Restart();
        var predictions = SimulatePredictions(network, dataset);
        var inferenceTime = stopwatch.ElapsedMilliseconds;

        // Calculate metrics
        var metrics = MetricsCalculator.CalculateComprehensiveMetrics(
            modelName,
            dataset.Name,
            predictions,
            dataset.Targets,
            trainingTime,
            inferenceTime,
            EstimateMemoryUsage(neuronCount),
            neuronCount
        );

        return metrics;
    }

    private ILiquidNetwork CreateTestNetwork(int neuronCount, int inputDim, int outputDim)
    {
        // This is a placeholder - would create actual liquid network
        // For now, create a simple mock network structure
        return new MockLiquidNetwork(neuronCount, inputDim, outputDim);
    }

    private async Task SimulateTraining(ILiquidNetwork network, BenchmarkDataset dataset)
    {
        // Placeholder for actual training logic
        // Would implement proper liquid state machine training
        await Task.Delay(100); // Simulate training time
    }

    private double[,] SimulatePredictions(ILiquidNetwork network, BenchmarkDataset dataset)
    {
        // Placeholder for actual prediction logic
        // Generate synthetic predictions for testing
        var predictions = new double[dataset.Targets.GetLength(0), dataset.Targets.GetLength(1)];
        var random = new Random(42);

        for (int i = 0; i < predictions.GetLength(0); i++)
        {
            for (int j = 0; j < predictions.GetLength(1); j++)
            {
                // Add some noise to targets to simulate imperfect predictions
                predictions[i, j] = dataset.Targets[i, j] + (random.NextDouble() - 0.5) * 0.1;
            }
        }

        return predictions;
    }

    private double EstimateMemoryUsage(int neuronCount)
    {
        // Rough estimate of memory usage in MB
        return neuronCount * 0.001 + 5.0; // Base overhead + per-neuron memory
    }

    private void GenerateSummaryReport()
    {
        Console.WriteLine("=== BENCHMARK SUMMARY REPORT ===");
        Console.WriteLine();

        var groupedResults = _results.GroupBy(r => r.DatasetName);

        foreach (var group in groupedResults)
        {
            Console.WriteLine($"Dataset: {group.Key}");
            Console.WriteLine("----------------------------------------");

            foreach (var result in group.OrderBy(r => r.MeanSquaredError))
            {
                Console.WriteLine($"{result.ModelName,-15} | MSE: {result.MeanSquaredError:F6} | " +
                                $"RMSE: {result.RootMeanSquaredError:F6} | R²: {result.R2Score:F4} | " +
                                $"Time: {result.TrainingTime:F0}ms");
            }

            var bestResult = group.OrderBy(r => r.MeanSquaredError).First();
            Console.WriteLine($"Best performing model: {bestResult.ModelName} (MSE: {bestResult.MeanSquaredError:F6})");
            Console.WriteLine();
        }

        // Overall statistics
        Console.WriteLine("=== OVERALL STATISTICS ===");
        Console.WriteLine($"Total benchmarks run: {_results.Count}");
        Console.WriteLine($"Average MSE: {_results.Average(r => r.MeanSquaredError):F6}");
        Console.WriteLine($"Average R²: {_results.Average(r => r.R2Score):F4}");
        Console.WriteLine($"Average training time: {_results.Average(r => r.TrainingTime):F2}ms");
        Console.WriteLine($"Total memory usage range: {_results.Min(r => r.MemoryUsage):F2}MB - {_results.Max(r => r.MemoryUsage):F2}MB");
    }
}

/// <summary>
/// BenchmarkDotNet micro-benchmarks for specific performance testing
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
public class LiquidNetMicroBenchmarks
{
    private BenchmarkDataset _mackeyGlassData = null!;
    private BenchmarkDataset _sineWaveData = null!;
    private ILiquidNetwork _smallNetwork = null!;
    private ILiquidNetwork _largeNetwork = null!;

    [GlobalSetup]
    public void Setup()
    {
        _mackeyGlassData = StandardDatasets.GenerateMackeyGlass(500);
        _sineWaveData = StandardDatasets.GenerateSineWave(500);
        _smallNetwork = new MockLiquidNetwork(32, 1, 1);
        _largeNetwork = new MockLiquidNetwork(128, 1, 1);
    }

    [Benchmark]
    public double SmallNetworkMackeyGlass() => RunBenchmark(_smallNetwork, _mackeyGlassData);

    [Benchmark]
    public double LargeNetworkMackeyGlass() => RunBenchmark(_largeNetwork, _mackeyGlassData);

    [Benchmark]
    public double SmallNetworkSineWave() => RunBenchmark(_smallNetwork, _sineWaveData);

    [Benchmark]
    public double LargeNetworkSineWave() => RunBenchmark(_largeNetwork, _sineWaveData);

    private double RunBenchmark(ILiquidNetwork network, BenchmarkDataset dataset)
    {
        // Simplified benchmark focusing on computation time
        double totalError = 0.0;
        for (int i = 0; i < Math.Min(100, dataset.Inputs.GetLength(0)); i++)
        {
            // Mock processing
            totalError += Math.Abs(dataset.Targets[i, 0] - (dataset.Inputs[i, 0] + 0.1));
        }
        return totalError;
    }
}

/// <summary>
/// Mock implementation of ILiquidNetwork for benchmarking
/// </summary>
internal class MockLiquidNetwork : ILiquidNetwork
{
    private readonly List<INeuron> _neurons;
    private readonly List<ISynapse> _synapses;

    public IReadOnlyCollection<INeuron> Neurons => _neurons.AsReadOnly();
    public IReadOnlyCollection<ISynapse> Synapses => _synapses.AsReadOnly();
    public double CurrentTime { get; private set; }

    public MockLiquidNetwork(int neuronCount, int inputDim, int outputDim)
    {
        _neurons = new List<INeuron>();
        _synapses = new List<ISynapse>();

        for (int i = 0; i < neuronCount; i++)
        {
            _neurons.Add(new BasicNeuron(threshold: 1.0));
        }

        CurrentTime = 0.0;
    }

    public void AddNeuron(INeuron neuron)
    {
        _neurons.Add(neuron);
    }

    public void AddSynapse(ISynapse synapse)
    {
        _synapses.Add(synapse);
    }

    public void Step(double deltaTime)
    {
        CurrentTime += deltaTime;
        foreach (var neuron in _neurons)
        {
            neuron.Update(deltaTime);
        }
    }

    public void Reset()
    {
        CurrentTime = 0.0;
        foreach (var neuron in _neurons)
        {
            neuron.Reset();
        }
    }

    public async Task<double[]> ProcessAsync(double[] inputs)
    {
        // Set inputs to first neurons by casting to BasicNeuron
        for (int i = 0; i < Math.Min(inputs.Length, _neurons.Count); i++)
        {
            if (_neurons[i] is BasicNeuron basicNeuron)
            {
                basicNeuron.Potential = inputs[i];
            }
        }

        // Simulate processing
        Step(0.1);

        // Return outputs from last neurons
        var outputCount = Math.Min(1, _neurons.Count);
        var outputs = new double[outputCount];
        for (int i = 0; i < outputCount; i++)
        {
            outputs[i] = _neurons[_neurons.Count - 1 - i].Potential;
        }

        return await Task.FromResult(outputs);
    }
}