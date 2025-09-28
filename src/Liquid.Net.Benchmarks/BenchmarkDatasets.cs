using System;
using System.Collections.Generic;

namespace Liquid.Net.Benchmarks.Data;

/// <summary>
/// Represents a benchmark dataset used for evaluating LNN performance
/// </summary>
public class BenchmarkDataset
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public double[,] Inputs { get; init; } = new double[0, 0];
    public double[,] Targets { get; init; } = new double[0, 0];
    public int SequenceLength { get; init; }
    public int InputDimension { get; init; }
    public int OutputDimension { get; init; }
    public string Source { get; init; } = string.Empty;
}

/// <summary>
/// Standard benchmark datasets used for LNN evaluation
/// </summary>
public static class StandardDatasets
{
    /// <summary>
    /// Generate synthetic time series data for sequence prediction
    /// Based on the Mackey-Glass equation used in original LNN papers
    /// </summary>
    public static BenchmarkDataset GenerateMackeyGlass(int length = 1000, double tau = 17.0)
    {
        var data = new double[length];
        var x = 1.2; // Initial condition
        var dt = 0.1;

        for (int i = 0; i < length; i++)
        {
            var delayed = i < (int)(tau / dt) ? 0.1 : data[i - (int)(tau / dt)];
            var dx = (0.2 * delayed) / (1 + Math.Pow(delayed, 10)) - 0.1 * x;
            x += dt * dx;
            data[i] = x;
        }

        // Create input-output pairs for sequence prediction
        int sequenceLength = 20;
        int samples = length - sequenceLength - 1;
        var inputs = new double[samples, sequenceLength];
        var targets = new double[samples, 1];

        for (int i = 0; i < samples; i++)
        {
            for (int j = 0; j < sequenceLength; j++)
            {
                inputs[i, j] = data[i + j];
            }
            targets[i, 0] = data[i + sequenceLength];
        }

        return new BenchmarkDataset
        {
            Name = "Mackey-Glass",
            Description = "Chaotic time series prediction using Mackey-Glass equation",
            Inputs = inputs,
            Targets = targets,
            SequenceLength = sequenceLength,
            InputDimension = 1,
            OutputDimension = 1,
            Source = "Mackey, M. C. & Glass, L. (1977). Oscillation and chaos in physiological control systems"
        };
    }

    /// <summary>
    /// Generate synthetic sine wave data for continuous learning
    /// </summary>
    public static BenchmarkDataset GenerateSineWave(int length = 1000, double frequency = 0.1, double amplitude = 1.0, double noise = 0.1)
    {
        var random = new Random(42); // Fixed seed for reproducibility
        var data = new double[length];

        for (int i = 0; i < length; i++)
        {
            var t = i * 0.1;
            data[i] = amplitude * Math.Sin(2 * Math.PI * frequency * t) + noise * (random.NextDouble() - 0.5);
        }

        // Create input-output pairs
        int sequenceLength = 10;
        int samples = length - sequenceLength - 1;
        var inputs = new double[samples, sequenceLength];
        var targets = new double[samples, 1];

        for (int i = 0; i < samples; i++)
        {
            for (int j = 0; j < sequenceLength; j++)
            {
                inputs[i, j] = data[i + j];
            }
            targets[i, 0] = data[i + sequenceLength];
        }

        return new BenchmarkDataset
        {
            Name = "Sine Wave",
            Description = "Noisy sine wave prediction for continuous learning evaluation",
            Inputs = inputs,
            Targets = targets,
            SequenceLength = sequenceLength,
            InputDimension = 1,
            OutputDimension = 1,
            Source = "Synthetic"
        };
    }

    /// <summary>
    /// Generate Lorenz attractor data for complex dynamical system modeling
    /// </summary>
    public static BenchmarkDataset GenerateLorenzAttractor(int length = 2000, double sigma = 10.0, double rho = 28.0, double beta = 8.0 / 3.0)
    {
        var data = new double[length, 3]; // x, y, z coordinates
        var x = 1.0; var y = 1.0; var z = 1.0; // Initial conditions
        var dt = 0.01;

        for (int i = 0; i < length; i++)
        {
            var dx = sigma * (y - x);
            var dy = x * (rho - z) - y;
            var dz = x * y - beta * z;

            x += dt * dx;
            y += dt * dy;
            z += dt * dz;

            data[i, 0] = x;
            data[i, 1] = y;
            data[i, 2] = z;
        }

        // Create sequence prediction task
        int sequenceLength = 15;
        int samples = length - sequenceLength - 1;
        var inputs = new double[samples, sequenceLength * 3]; // Flattened xyz sequences
        var targets = new double[samples, 3]; // Next xyz coordinates

        for (int i = 0; i < samples; i++)
        {
            for (int j = 0; j < sequenceLength; j++)
            {
                inputs[i, j * 3] = data[i + j, 0];
                inputs[i, j * 3 + 1] = data[i + j, 1];
                inputs[i, j * 3 + 2] = data[i + j, 2];
            }
            targets[i, 0] = data[i + sequenceLength, 0];
            targets[i, 1] = data[i + sequenceLength, 1];
            targets[i, 2] = data[i + sequenceLength, 2];
        }

        return new BenchmarkDataset
        {
            Name = "Lorenz Attractor",
            Description = "Chaotic dynamical system modeling using Lorenz equations",
            Inputs = inputs,
            Targets = targets,
            SequenceLength = sequenceLength,
            InputDimension = 3,
            OutputDimension = 3,
            Source = "Lorenz, E. N. (1963). Deterministic nonperiodic flow"
        };
    }

    /// <summary>
    /// Get all standard benchmark datasets
    /// </summary>
    public static IEnumerable<BenchmarkDataset> GetAllDatasets()
    {
        yield return GenerateMackeyGlass();
        yield return GenerateSineWave();
        yield return GenerateLorenzAttractor();
    }
}