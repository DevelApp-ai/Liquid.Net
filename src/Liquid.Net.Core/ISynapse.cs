using System;

namespace Liquid.Net.Core;

/// <summary>
/// Represents a synapse (connection) between two neurons in a liquid neural network.
/// </summary>
public interface ISynapse
{
    /// <summary>
    /// Unique identifier for the synapse.
    /// </summary>
    Guid Id { get; }
    
    /// <summary>
    /// The neuron that sends signals through this synapse.
    /// </summary>
    INeuron PresynapticNeuron { get; }
    
    /// <summary>
    /// The neuron that receives signals through this synapse.
    /// </summary>
    INeuron PostsynapticNeuron { get; }
    
    /// <summary>
    /// Current weight (strength) of the synaptic connection.
    /// </summary>
    double Weight { get; set; }
    
    /// <summary>
    /// Timestamp of the last signal transmission.
    /// </summary>
    double LastTransmission { get; }
    
    /// <summary>
    /// Transmits a signal through the synapse.
    /// </summary>
    /// <param name="signal">The signal strength to transmit.</param>
    /// <param name="deltaTime">Time elapsed since last transmission.</param>
    void Transmit(double signal, double deltaTime);
    
    /// <summary>
    /// Updates the synaptic weight.
    /// </summary>
    /// <param name="deltaWeight">Change in weight to apply.</param>
    void UpdateWeight(double deltaWeight);
}