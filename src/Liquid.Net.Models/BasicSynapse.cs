using System;
using Liquid.Net.Core;

namespace Liquid.Net.Models;

/// <summary>
/// Basic implementation of a synapse with simple transmission and plasticity.
/// </summary>
public class BasicSynapse : ISynapse
{
    public BasicSynapse(INeuron presynapticNeuron, INeuron postsynapticNeuron, double initialWeight = 0.5)
    {
        Id = Guid.NewGuid();
        PresynapticNeuron = presynapticNeuron ?? throw new ArgumentNullException(nameof(presynapticNeuron));
        PostsynapticNeuron = postsynapticNeuron ?? throw new ArgumentNullException(nameof(postsynapticNeuron));
        Weight = initialWeight;
        LastTransmission = 0.0;
    }

    public Guid Id { get; }
    public INeuron PresynapticNeuron { get; }
    public INeuron PostsynapticNeuron { get; }
    public double Weight { get; set; }
    public double LastTransmission { get; private set; }

    /// <summary>
    /// Transmission delay in time units.
    /// </summary>
    public double Delay { get; set; } = 1.0;

    /// <summary>
    /// Maximum weight value to prevent runaway growth.
    /// </summary>
    public double MaxWeight { get; set; } = 10.0;

    /// <summary>
    /// Minimum weight value to prevent negative weights.
    /// </summary>
    public double MinWeight { get; set; } = 0.0;

    public void Transmit(double signal, double deltaTime)
    {
        LastTransmission = deltaTime;

        // Simple transmission: multiply signal by weight
        var transmittedSignal = signal * Weight;

        // In a full implementation, this would be queued with delay
        // For now, we just record the transmission
    }

    public void UpdateWeight(double deltaWeight)
    {
        Weight += deltaWeight;

        // Clamp weight within bounds
        Weight = Math.Max(MinWeight, Math.Min(MaxWeight, Weight));
    }

    /// <summary>
    /// Applies Hebbian learning rule: strengthen connections between co-active neurons.
    /// </summary>
    /// <param name="learningRate">Rate of weight change.</param>
    public void ApplyHebbianLearning(double learningRate)
    {
        if (PresynapticNeuron.IsActive && PostsynapticNeuron.IsActive)
        {
            UpdateWeight(learningRate);
        }
    }

    /// <summary>
    /// Applies weight decay to prevent unlimited growth.
    /// </summary>
    /// <param name="decayRate">Rate of weight decay.</param>
    public void ApplyWeightDecay(double decayRate)
    {
        Weight *= (1.0 - decayRate);
        Weight = Math.Max(MinWeight, Weight);
    }
}