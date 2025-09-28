using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.Net.Benchmarks.Metrics;

/// <summary>
/// Performance metrics for evaluating LNN model performance
/// </summary>
public class BenchmarkResults
{
    public string ModelName { get; init; } = string.Empty;
    public string DatasetName { get; init; } = string.Empty;
    public double MeanSquaredError { get; init; }
    public double RootMeanSquaredError { get; init; }
    public double MeanAbsoluteError { get; init; }
    public double NormalizedRootMeanSquaredError { get; init; }
    public double R2Score { get; init; }
    public double TrainingTime { get; init; }
    public double InferenceTime { get; init; }
    public double MemoryUsage { get; init; }
    public int NetworkSize { get; init; }
    public Dictionary<string, object> AdditionalMetrics { get; init; } = new();
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}

/// <summary>
/// Utility class for calculating performance metrics
/// </summary>
public static class MetricsCalculator
{
    /// <summary>
    /// Calculate Mean Squared Error (MSE)
    /// </summary>
    public static double CalculateMSE(double[,] predictions, double[,] targets)
    {
        if (predictions.GetLength(0) != targets.GetLength(0) ||
            predictions.GetLength(1) != targets.GetLength(1))
            throw new ArgumentException("Predictions and targets must have the same dimensions");

        double sumSquaredError = 0.0;
        int totalElements = predictions.GetLength(0) * predictions.GetLength(1);

        for (int i = 0; i < predictions.GetLength(0); i++)
        {
            for (int j = 0; j < predictions.GetLength(1); j++)
            {
                double error = predictions[i, j] - targets[i, j];
                sumSquaredError += error * error;
            }
        }

        return sumSquaredError / totalElements;
    }

    /// <summary>
    /// Calculate Root Mean Squared Error (RMSE)
    /// </summary>
    public static double CalculateRMSE(double[,] predictions, double[,] targets)
    {
        return Math.Sqrt(CalculateMSE(predictions, targets));
    }

    /// <summary>
    /// Calculate Mean Absolute Error (MAE)
    /// </summary>
    public static double CalculateMAE(double[,] predictions, double[,] targets)
    {
        if (predictions.GetLength(0) != targets.GetLength(0) ||
            predictions.GetLength(1) != targets.GetLength(1))
            throw new ArgumentException("Predictions and targets must have the same dimensions");

        double sumAbsoluteError = 0.0;
        int totalElements = predictions.GetLength(0) * predictions.GetLength(1);

        for (int i = 0; i < predictions.GetLength(0); i++)
        {
            for (int j = 0; j < predictions.GetLength(1); j++)
            {
                sumAbsoluteError += Math.Abs(predictions[i, j] - targets[i, j]);
            }
        }

        return sumAbsoluteError / totalElements;
    }

    /// <summary>
    /// Calculate Normalized Root Mean Squared Error (NRMSE)
    /// </summary>
    public static double CalculateNRMSE(double[,] predictions, double[,] targets)
    {
        var rmse = CalculateRMSE(predictions, targets);
        var targetRange = CalculateRange(targets);
        return targetRange > 0 ? rmse / targetRange : double.NaN;
    }

    /// <summary>
    /// Calculate R-squared (coefficient of determination)
    /// </summary>
    public static double CalculateR2Score(double[,] predictions, double[,] targets)
    {
        double targetMean = CalculateMean(targets);
        double totalSumSquares = 0.0;
        double residualSumSquares = 0.0;

        for (int i = 0; i < targets.GetLength(0); i++)
        {
            for (int j = 0; j < targets.GetLength(1); j++)
            {
                double targetDeviation = targets[i, j] - targetMean;
                double residual = targets[i, j] - predictions[i, j];

                totalSumSquares += targetDeviation * targetDeviation;
                residualSumSquares += residual * residual;
            }
        }

        return totalSumSquares > 0 ? 1.0 - (residualSumSquares / totalSumSquares) : double.NaN;
    }

    /// <summary>
    /// Calculate comprehensive metrics for a prediction vs target comparison
    /// </summary>
    public static BenchmarkResults CalculateComprehensiveMetrics(
        string modelName,
        string datasetName,
        double[,] predictions,
        double[,] targets,
        double trainingTime = 0.0,
        double inferenceTime = 0.0,
        double memoryUsage = 0.0,
        int networkSize = 0)
    {
        var mse = CalculateMSE(predictions, targets);
        var rmse = Math.Sqrt(mse);
        var mae = CalculateMAE(predictions, targets);
        var nrmse = CalculateNRMSE(predictions, targets);
        var r2 = CalculateR2Score(predictions, targets);

        return new BenchmarkResults
        {
            ModelName = modelName,
            DatasetName = datasetName,
            MeanSquaredError = mse,
            RootMeanSquaredError = rmse,
            MeanAbsoluteError = mae,
            NormalizedRootMeanSquaredError = nrmse,
            R2Score = r2,
            TrainingTime = trainingTime,
            InferenceTime = inferenceTime,
            MemoryUsage = memoryUsage,
            NetworkSize = networkSize
        };
    }

    /// <summary>
    /// Calculate prediction horizon accuracy (for time series)
    /// </summary>
    public static Dictionary<int, double> CalculatePredictionHorizonAccuracy(
        double[,] predictions,
        double[,] targets,
        int maxHorizon = 10)
    {
        var horizonAccuracy = new Dictionary<int, double>();

        for (int horizon = 1; horizon <= Math.Min(maxHorizon, predictions.GetLength(0)); horizon++)
        {
            double totalError = 0.0;
            int count = 0;

            for (int i = horizon - 1; i < predictions.GetLength(0); i++)
            {
                for (int j = 0; j < predictions.GetLength(1); j++)
                {
                    double error = Math.Abs(predictions[i, j] - targets[i, j]);
                    totalError += error;
                    count++;
                }
            }

            horizonAccuracy[horizon] = count > 0 ? totalError / count : double.NaN;
        }

        return horizonAccuracy;
    }

    private static double CalculateMean(double[,] array)
    {
        double sum = 0.0;
        int count = array.GetLength(0) * array.GetLength(1);

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                sum += array[i, j];
            }
        }

        return sum / count;
    }

    private static double CalculateRange(double[,] array)
    {
        double min = double.MaxValue;
        double max = double.MinValue;

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                min = Math.Min(min, array[i, j]);
                max = Math.Max(max, array[i, j]);
            }
        }

        return max - min;
    }
}