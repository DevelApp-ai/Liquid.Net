using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.Net.Core;

/// <summary>
/// Represents a liquid neural network containing neurons and synapses.
/// </summary>
public interface ILiquidNetwork
{
    /// <summary>
    /// Collection of all neurons in the network.
    /// </summary>
    IReadOnlyCollection<INeuron> Neurons { get; }
    
    /// <summary>
    /// Collection of all synapses in the network.
    /// </summary>
    IReadOnlyCollection<ISynapse> Synapses { get; }
    
    /// <summary>
    /// Current simulation time of the network.
    /// </summary>
    double CurrentTime { get; }
    
    /// <summary>
    /// Adds a neuron to the network.
    /// </summary>
    /// <param name="neuron">The neuron to add.</param>
    void AddNeuron(INeuron neuron);
    
    /// <summary>
    /// Adds a synapse to the network.
    /// </summary>
    /// <param name="synapse">The synapse to add.</param>
    void AddSynapse(ISynapse synapse);
    
    /// <summary>
    /// Advances the network simulation by one time step.
    /// </summary>
    /// <param name="deltaTime">Time step duration.</param>
    void Step(double deltaTime);
    
    /// <summary>
    /// Resets the network to its initial state.
    /// </summary>
    void Reset();
    
    /// <summary>
    /// Processes input signals through the network asynchronously.
    /// </summary>
    /// <param name="inputs">Input signal array.</param>
    /// <returns>Output signal array.</returns>
    Task<double[]> ProcessAsync(double[] inputs);
}